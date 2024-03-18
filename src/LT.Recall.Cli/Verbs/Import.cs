using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using System.Diagnostics.CodeAnalysis;

namespace LT.Recall.Cli.Verbs
{
    internal class Import : Verb<Import.Options>
    {
        public readonly Application.Features.Import.Handler _importHandler;

        public Import(Application.Features.Import.Handler importHandler)
        {
            _importHandler = importHandler;
        }

        public class Options : Program.Options
        {
            [method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Options))]
            public Options()
            {

            }
            public string Path { get; set; } = string.Empty;
        }


        protected override string HelpText => Resources.ImportHelpText;

        protected override async Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            var response = await _importHandler.Handle(new Application.Features.Import.Request
            {
                FilePath = options.Path
            }, CancellationToken.None);

            return new CliResult(response.UserFriendlyMessage, ResultType.Success, response);
        }
    }
}
