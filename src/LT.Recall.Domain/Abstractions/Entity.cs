namespace LT.Recall.Domain.Abstractions
{
    public abstract class Entity : IEntity
    {
        // private Regex _idRegex = new(@"[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?", RegexOptions.Compiled);
        public virtual string? Id { get; private set; }
        public bool HasId => !string.IsNullOrWhiteSpace(Id);

        protected Entity(string? id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                Id = id;
            }
        }

        protected virtual string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public void GenerateIdIfEmpty()
        {
            if (!HasId)
                Id = GenerateId();

            if(!HasId)
                throw new InvalidOperationException("Failed to generate ID");
        }
    }
}
