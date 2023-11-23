namespace LT.Recall.IntegrationTests.Debuggers;

public interface IDebugger
{
    void Attach(int pid);
}