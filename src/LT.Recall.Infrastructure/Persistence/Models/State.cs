using LT.Recall.Domain.Entities;

namespace LT.Recall.Infrastructure.Persistence.Models
{
    public class State
    {
        public List<Command> Commands { get; set; } = new List<Command>();
    }
}
