namespace LT.Recall.Infrastructure
{
    public class InfrastructureConfiguration(bool xTest)
    {
        public string StateFilePath { get; init; } = xTest
            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recall-state.json")
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RecallCli", "recall-state.json");
    }
}
