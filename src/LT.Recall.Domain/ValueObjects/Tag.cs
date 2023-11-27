namespace LT.Recall.Domain.ValueObjects
{
    public class Tag : IEquatable<Tag>
    {
        public Tag(string name)
        {
            Name = name.Trim();
        }

        public string Name { get; init; }

        public bool Equals(Tag? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name.ToLowerInvariant() == other.Name.ToLowerInvariant();
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tag)obj);
        }

        public override int GetHashCode()
        {
            return Name.ToLowerInvariant().GetHashCode();
        }
    }
}
