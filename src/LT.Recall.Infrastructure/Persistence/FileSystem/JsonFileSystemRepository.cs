using LT.Recall.Application.Abstractions;
using LT.Recall.Application.Errors;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;
using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.Persistence.Models;
using LT.Recall.Infrastructure.Persistence.Search;
using LT.Recall.Infrastructure.Properties;
using System.Text;
using System.Text.RegularExpressions;

namespace LT.Recall.Infrastructure.Persistence.FileSystem
{
    /// <summary>
    /// File System persistence implementation of <see cref="ICommandRepository"/>
    /// </summary>
    public class JsonFileSystemRepository : ICommandRepository
    {
        private static readonly Encoding Encoding = Encoding.UTF8;
        private string? _filePath;
        private string _rollbackFilePath = string.Empty;

        private readonly IJsonSerializer _jsonSerializer;
        private readonly InfrastructureConfiguration _infrastructureConfiguration;

        public JsonFileSystemRepository(IJsonSerializer jsonSerializer, InfrastructureConfiguration infrastructureConfiguration)
        {
            _jsonSerializer = jsonSerializer;
            _infrastructureConfiguration = infrastructureConfiguration;
        }

        public async Task<(int updated, int inserted)> BulkUpsert(List<Command> commands)
        {
            int inserted = 0;
            int updated = 0;

            try
            {
                await BeginTransactionAsync();

                foreach (var command in commands)
                {
                    ThrowIfDuplicate(commands, command);

                    command.GenerateIdIfEmpty();

                    var exists = await ExistsAsync(command.Id ?? string.Empty);
                    if (exists)
                    {
                        await UpdateAsync(command);
                        updated++;
                    }
                    else
                    {
                        await SaveAsync(command);
                        inserted++;
                    }
                }

                await CommitTransactionAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }

            return (updated, inserted);
        }

        private static void ThrowIfDuplicate(List<Command> commands, Command command)
        {
            if (commands.Count(x => x.CommandId == command.CommandId && string.Equals(x.Collection, command.Collection, StringComparison.InvariantCultureIgnoreCase)) > 1)
            {
                throw new ValidationError(string.Format(Resources.DuplicateCommandError, command.CommandId));
            }
        }

        public Task BeginTransactionAsync()
        {
            var stateFile = GetStateFilePath();
            var directory = Path.GetDirectoryName(stateFile);
            _rollbackFilePath = Path.Combine(directory!, Guid.NewGuid().ToString());
            File.Copy(stateFile, _rollbackFilePath);
            return Task.CompletedTask;
        }

        public Task CommitTransactionAsync()
        {
            if (string.IsNullOrWhiteSpace(_rollbackFilePath)) throw new InfrastructureError(InfraErrorCode.NoTransactionInProgress);
            File.Delete(_rollbackFilePath);
            return Task.CompletedTask;
        }

        public Task RollbackTransactionAsync()
        {
            if (string.IsNullOrWhiteSpace(_rollbackFilePath)) throw new InfrastructureError(InfraErrorCode.NoTransactionInProgress);
            File.Copy(_rollbackFilePath, GetStateFilePath(), true);
            File.Delete(_rollbackFilePath);
            return Task.CompletedTask;
        }

        public Task<string> SaveAsync(Command command)
        {
            command.GenerateIdIfEmpty();
            var state = GetState();
            if (state.Commands.Any(x => x.Id == command.Id))
            {
                throw new InfrastructureError(string.Format(Resources.DuplicateCommandError, command.Id), InfraErrorCode.UniqueConstraintViolation);
            }
            state.Commands.Add(command);
            SaveState(state);
            return Task.FromResult(command.Id)!;
        }

        public Task UpdateAsync(Command command)
        {
            command.GenerateIdIfEmpty();
            var state = GetState();
            var index = state.Commands.FindIndex(x => string.Equals(command.Id, x.Id, StringComparison.InvariantCultureIgnoreCase));
            if (index == -1)
            {
                throw new InfrastructureError(string.Format(Resources.ItemNotFoundError, nameof(Command), command.Id), InfraErrorCode.NotFound);
            }
            state.Commands[index] = command;
            SaveState(state);
            return Task.CompletedTask;
        }

        public Task<(List<Command>? commands, int totalResults)> SearchAsync(string searchString, int page, int pageSize)
        {
            var searchModel = SearchStringParser.Parse(searchString);
            var query = GetState().Commands.AsQueryable();

            query = query.Where(x => searchModel.Terms.Any(term => x.SearchableText.Contains(term, StringComparison.InvariantCultureIgnoreCase)));
            query = query.OrderByDescending(x => SearchRank(x, searchModel));

            return Page(page, pageSize, query.ToList());
        }

        public Task<(List<Command>? commands, int totalResults)> FetchAllAsync(int page, int pageSize)
        {
            var allCommands = GetState().Commands;
            return Page(page, pageSize, allCommands);
        }

        public Task<List<Command>> FetchAsync(string collection, List<Tag> tags)
        {
            var state = GetState();
            return Task.FromResult(state.Commands.Where(x =>
                string.Equals(x.Collection, collection, StringComparison.InvariantCultureIgnoreCase) ||
                x.Tags.Any(tags.Contains)).ToList());
        }

        public Task DeleteAsync(List<string> commandIds)
        {
            var state = GetState();
            state.Commands.RemoveAll(x => commandIds.Contains(x.Id!));
            SaveState(state);
            return Task.CompletedTask;
        }

        public Task<List<Command>> FetchByCollectionAsync(string collection)
        {
            var state = GetState();
            return Task.FromResult(state.Commands.Where(x => string.Equals(x.Collection, collection, StringComparison.InvariantCultureIgnoreCase)).ToList());
        }

        public Task<bool> ExistsAsync(string id)
        {
            var state = GetState();
            return Task.FromResult(state.Commands.Any(x => string.Equals(id, x.Id, StringComparison.InvariantCultureIgnoreCase)));
        }

        /// <summary>
        /// Simple search rank based on the # of occurrences of the search terms in the command text
        /// Future iteration could include proximity score also (i.e. if the terms are close together in the command text)
        /// </summary>
        private int SearchRank(Command command, SearchModel searchModel)
        {
            var rank = 0;
            foreach (var term in searchModel.Terms)
            {
                var matches = Regex.Matches(command.SearchableText, @$"{term}", RegexOptions.IgnoreCase);
                foreach (Match unused in matches)
                {
                    rank++;
                }
            }
            return rank;
        }

        private Task<(List<Command>? commands, int totalResults)> Page(int page, int pageSize, List<Command> filtered)
        {
            return Task.FromResult<(List<Command>? commands, int totalResults)>(
                new(filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList(), filtered.Count));
        }

        private string GetStateFilePath()
        {
            if (_filePath == null)
            {
                _filePath = _infrastructureConfiguration.StateFilePath;
            }

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }

            return _filePath;
        }

        private State GetState()
        {
            return _jsonSerializer.Deserialize<State>(File.ReadAllText(GetStateFilePath(), Encoding)) ?? new State();
        }

        private void SaveState(State state)
        {
            File.WriteAllBytes(GetStateFilePath(), Encoding.GetBytes(_jsonSerializer.Serialize(state)));
        }
    }
}
