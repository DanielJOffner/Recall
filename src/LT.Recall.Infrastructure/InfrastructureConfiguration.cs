namespace LT.Recall.Infrastructure
{
    public class InfrastructureConfiguration
    {
        public string StateFilePath { get; init; } = Path.Combine(Directory.GetCurrentDirectory(), "recall-state.json");
    }
}
