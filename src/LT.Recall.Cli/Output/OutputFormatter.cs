using LT.Recall.Application.Abstractions;
using LT.Recall.Cli.Themes;

namespace LT.Recall.Cli.Output
{
    internal static class OutputFormatter
    {
        public static void Write(ITheme theme, CliResult cliResult, bool isTestMode, IJsonSerializer serializer)
        {
            if (isTestMode)
            {
                FormatTestOutput(cliResult, serializer);
            }
            else
            {
                FormatOutput(theme, cliResult);
            }
        }

        private static void FormatOutput(ITheme theme, CliResult cliResult)
        {
            switch (cliResult.ResultType)
            {
                case ResultType.Error:
                    Console.ForegroundColor = theme.Error;
                    break;
                case ResultType.Warning:
                    Console.ForegroundColor = theme.Warning;
                    break;
                case ResultType.Success:
                    Console.ForegroundColor = theme.Success;
                    break;
                case ResultType.Message:
                    Console.ForegroundColor = theme.Message;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!string.IsNullOrWhiteSpace(cliResult.Message))
            {
                Console.WriteLine(cliResult.Message);
                Console.ResetColor();
            }
        }

        private static void FormatTestOutput(CliResult cliResult, IJsonSerializer serializer)
        {
            // Console.Clear();
            Console.WriteLine(serializer.Serialize(cliResult));
        }
    }
}
