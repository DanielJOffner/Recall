using LT.Recall.IntegrationTests.Debuggers;
using System.Diagnostics;

namespace LT.Recall.IntegrationTests.Fixtures
{
    [SetUpFixture]
    internal class CliFixture
    {
        private readonly int _processTimeout = 2000;
        private readonly string _fileName = Environment.OSVersion.Platform == PlatformID.Unix ? "LT.Recall.Cli" : "LT.Recall.Cli.exe";
        private readonly string _testOption = "--xtest";
        private readonly string _verboseOption = "--verbose";

        internal string ExecuteCli(string args)
        {
            return ExecuteCli(listArgs: null, stringArgs: args);
        }

        internal string ExecuteCli(string[] args)
        {
            return ExecuteCli(listArgs: args, stringArgs: null);
        }

        internal string ExecuteCliSequence(string args, List<string> inputSequence)
        {
            return ExecuteSequence(inputSequence, listArgs: null, stringArgs: args);
        }

        internal string ExecuteCliSequence(string[] args, List<string> inputSequence)
        {
            return ExecuteSequence(inputSequence, listArgs: args, stringArgs: null);
        }

        private string ExecuteSequence(List<string> inputSequence, string[]? listArgs = null, string? stringArgs = null)
        {
            var process = StartCli(listArgs, stringArgs);
            foreach (var input in inputSequence)
            {
                process.StandardInput.WriteLine(input);
            }
            return WaitForOutput(process);
        }

        private string ExecuteCli(string[]? listArgs = null, string? stringArgs = null)
        {
            var process = StartCli(listArgs, stringArgs);
            return WaitForOutput(process);
        }

        private string WaitForOutput(Process process)
        {
            if (!IsDebugMode())
            {
                // don't want to wait for exit if in debug mode because the user will be stepping through the code
                if (!process.WaitForExit(_processTimeout))
                    throw new Exception($"CLI failed to exit after {_processTimeout}ms");
            }
            return process.StandardOutput.ReadToEnd();
        }

        private Process StartCli(string[]? listArgs, string? stringArgs)
        {
            var process = new Process();
            process.StartInfo.Arguments = GetArguments(listArgs, stringArgs);
            process.StartInfo.FileName = _fileName;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            if (IsDebugMode())
            {
                DebuggerFactory.GetDebugger().Attach(process.Id);
            }
            process.StandardInput.WriteLine(string.Empty); // when running in test mode, CLI always requires some input before starting
            return process;
        }

        private string GetArguments(string[]? listArgs, string? stringArgs)
        {
            string arguments;
            if (listArgs != null)
            {
                arguments = GetArguments(listArgs);
            }
            else if (stringArgs != null)
            {
                arguments = GetArguments(stringArgs);
            }
            else
            {
                throw new ArgumentException("Either listArgs or stringArgs must be provided");
            }
            return arguments;
        }

        private string GetArguments(string stringArgs)
        {
            return $"{_testOption} {_verboseOption} {stringArgs}";
        }

        private string GetArguments(string[] listArgs)
        {
            return string.Join(" ", listArgs.Prepend(_testOption).Prepend(_verboseOption).ToArray());
        }

        private static bool IsDebugMode()
        {
            if (Debugger.IsAttached)
            {
                return true;
            }

            return false;
        }
    }
}
