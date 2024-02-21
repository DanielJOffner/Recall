namespace LT.Recall.Infrastructure
{
    public class InfrastructureConfiguration
    {
        public string StateFilePath { get; init; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recall-state.json");
    }
}
