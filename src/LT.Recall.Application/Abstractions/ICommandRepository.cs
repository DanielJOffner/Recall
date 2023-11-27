using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;

namespace LT.Recall.Application.Abstractions
{
    public interface ICommandRepository
    {
        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        /// <returns>The ID of the new command</returns>
        Task<string> SaveAsync(Command command);
        Task UpdateAsync(Command command);
        Task<(List<Command>? commands, int totalResults)> SearchAsync(string searchString, int page, int pageSize);

        Task<(List<Command>? commands, int totalResults)> FetchAllAsync(int page, int pageSize);

        Task<List<Command>> FetchAsync(string collection, List<Tag> tags);
        Task DeleteAsync(List<string> commandIds);

        Task<List<Command>> FetchByCollectionAsync(string collection);

        Task<bool> ExistsAsync(string id);
    }
}
