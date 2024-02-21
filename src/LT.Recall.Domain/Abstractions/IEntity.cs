using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LT.Recall.Infrastructure")]
namespace LT.Recall.Domain.Abstractions
{
    public interface IEntity
    {
        string? Id { get; }
        public void GenerateIdIfEmpty();

        bool HasId { get; }
    }
}
