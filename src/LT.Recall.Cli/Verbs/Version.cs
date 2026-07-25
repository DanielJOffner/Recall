using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;

namespace LT.Recall.Cli.Verbs
{
    internal class Version : Verb<Version.Options>
    {
        protected override string HelpText => Resources.VersionHelpText;
        public class Options : Program.Options { }

        protected override Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "Unknown";
            return Task.FromResult(new CliResult(version));
        }
    }
}
