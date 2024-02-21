using System.Diagnostics;

namespace LT.Recall.IntegrationTests.Debuggers
{
    internal static class DebuggerFactory 
    {
        public static IDebugger GetDebugger()
        {
            Process[] procName1 = Process.GetProcessesByName("devenv");
            Process[] procName2 = Process.GetProcessesByName("msvsmon");
            if (procName1.Length > 0 || procName2.Length > 0)
            {
                return new VisualStudioDebugger();
            }

            throw new Exception("No visual studio process found. Tests currently only support visual studio debugger.");
        }
    }
}
