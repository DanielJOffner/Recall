using LT.Recall.Domain.Abstractions;
using LT.Recall.Domain.Properties;
using LT.Recall.Domain.ValueObjects;
using System.Text;

namespace LT.Recall.Domain.Entities
{
    public class Command : Entity
    {
        public Command(string commandText, string description, List<Tag>? tags = null, string? id = null, string? collection = null, int? commandId = null) : base(id)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentException(string.Format(Resources.InputRequiresValueError,
                    nameof(commandText)));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(string.Format(Resources.InputRequiresValueError,
                    nameof(description)));

            CommandText = commandText;
            Description = description;
            Tags = tags ?? new List<Tag>();
            Collection = string.IsNullOrWhiteSpace(collection) ? Resources.DefaultCollectionName : collection;
            CommandId = commandId ?? 0;
        }
        
        protected override string GenerateId()
        {
            return $"{Collection}_{CommandId}";
        }

        /// <summary>
        /// Set the CommandId to the next available value in the collection.
        /// </summary>
        public void SetCommandId(List<Command> collection)
        {
            if (!collection.Any())
            {
                CommandId = 1;
            }
            else
            {
                CommandId = collection.Max(x => x.CommandId) + 1;
            }
        }

        public int CommandId { get; private set; }
        public string Collection { get; init; }
        public string Description { get; init; }

        public string CommandText { get; init; }

        public List<Tag> Tags { get; init; } 

        public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;

        public string SearchableText
        {
            get
            {
                return $"{Description} {CommandText} {string.Join(" ", Tags.Select(tag => tag.Name))} {Collection}";
            }
        }

        /// <summary>
        /// Approximate size of the command in bytes assuming UTF8 encoding.
        /// </summary>
        public long Size => Encoding.UTF8.GetByteCount(SearchableText);
    }
}
