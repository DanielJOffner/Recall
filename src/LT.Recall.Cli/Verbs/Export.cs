using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using System.Diagnostics.CodeAnalysis;

namespace LT.Recall.Cli.Verbs
{
    internal class Export : Verb<Export.Options>
    {
        private readonly Application.Features.Export.Handler _exportHandler;

        public Export(Application.Features.Export.Handler exportHandler)
        {
            _exportHandler = exportHandler;
        }

        public class Options : Program.Options
        {
            [method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Options))]
            public Options()
            {

            }
            public string Path { get; set; } = string.Empty;
        }


        protected override string HelpText => Resources.ExportHelpText;

        protected override async Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            var response = await _exportHandler.Handle(new Application.Features.Export.Request()
            {
                FilePath = options.Path
            }, CancellationToken.None);
            return new CliResult(response.UserFriendlyMessage, ResultType.Success, response);
        }
    }
}
