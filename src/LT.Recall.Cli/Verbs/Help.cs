using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using System.Diagnostics.CodeAnalysis;

namespace LT.Recall.Cli.Verbs
{
    internal class Help : Verb<Help.Options>
    {
        public class Options : Program.Options
        {
            [method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Options))]
            public Options()
            {

            }
        }

        protected override string HelpText => Resources.HelpText;

        protected override Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            return Task.FromResult(new CliResult(HelpText));
        }
    }
}
