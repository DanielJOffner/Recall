using LT.Recall.Application.Abstractions;
using LT.Recall.Domain.Entities;
using LT.Recall.Infrastructure.Serialization;
using LT.Recall.IntegrationTests.Fixtures;
using System.Text.RegularExpressions;

namespace LT.Recall.IntegrationTests
{
    internal abstract class TestBase
    {
        private readonly PersistenceFixture PersistenceFixture = new PersistenceFixture();
        private readonly CliFixture CliFixture = new CliFixture();
        private readonly IJsonSerializer _jsonSerializer = new RecallJsonSerializer();

        [SetUp]
        public void Setup()
        {
            PersistenceFixture.Reset();
        }


        protected async Task SaveCommands(params Command[] commands)
        {
            foreach (var command in commands)
            {
                await SaveCommand(command);
            }
        }

        protected async Task SaveCommand(Command command)
        {
            await PersistenceFixture.Repository.SaveAsync(command);
        }

        protected async Task<(List<Command>? commands, int totalResults)> FetchAllCommands()
        {
            return await PersistenceFixture.Repository.FetchAllAsync(page:1, pageSize: 100);
        }

        protected TestModeCliResult<T> ExecuteCommand<T>(string args) where T : class, new()
        {
            var cliOutput = CliFixture.ExecuteCli(args);
            return ParseTestOutput<T>(cliOutput);
        }

        protected TestModeCliResult<T> ExecuteDeleteSequence<T>(string args) where T : class, new()
        {
            var cliOutput = CliFixture.ExecuteCliSequence(args, new List<string>(){ "Y" });
            return ParseTestOutput<T>(cliOutput);
        }

        protected TestModeCliResult<T> ExecuteSaveSequence<T>(string description, string commandText, string? tags = null, List<string>? args = null) where T : class, new()
        {
            string cliOutput;
            if (args == null || !args.Any())
            {
                cliOutput = CliFixture.ExecuteCliSequence("save", new List<string> { description, commandText, tags ?? string.Empty });
            }
            else
            {
                cliOutput = CliFixture.ExecuteCliSequence($"save {string.Join(" ", args)}", new List<string> { description, commandText, tags ?? string.Empty });
            }
            return ParseTestOutput<T>(cliOutput);
        }

        protected TestModeCliResult<T> ExecuteSequence<T>(string args) where T : class, new()
        {
            var cliOutput = CliFixture.ExecuteCli(args);
            return ParseTestOutput<T>(cliOutput);
        }

        protected TestModeCliResult<T> ExecuteCommand<T>(string[] args) where T : class, new()
        {
            var cliOutput = CliFixture.ExecuteCli(args);
            return ParseTestOutput<T>(cliOutput);
        }

        private TestModeCliResult<T> ParseTestOutput<T>(string cliOutput) where T : class, new()
        {
            var json = Regex.Match(cliOutput, @"{.*}");
            if (!json.Success)
                throw new Exception($"Failed to parse json from cli output. Output was {cliOutput}");

            return _jsonSerializer.Deserialize<TestModeCliResult<T>>(json.Value);
        }
    }
}
