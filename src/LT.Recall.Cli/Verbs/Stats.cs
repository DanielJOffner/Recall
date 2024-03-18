using LT.Recall.Cli.Output;
using LT.Recall.Cli.Properties;
using LT.Recall.Cli.Verbs.Base;
using System.Text;

namespace LT.Recall.Cli.Verbs
{
    internal class Stats : Verb<Stats.Options>
    {
        private readonly Application.Features.Stats.Handler _statsHandler;

        public Stats(Application.Features.Stats.Handler statsHandler)
        {
            _statsHandler = statsHandler;
        }

        public class Options : Program.Options { }

        protected override string HelpText => Resources.StatsHelpText;

        protected override async Task<CliResult> ExecuteInner(List<string> args, Options options)
        {

            var response = await _statsHandler.Handle(new Application.Features.Stats.Request(), CancellationToken.None);
            return new CliResult(GetMessage(response), ResultType.Success, response);
        }

        private string GetMessage(Application.Features.Stats.Response response)
        {
            var sb = new StringBuilder();
            FormatTotals(response, sb);
            FormatCollections(response, sb);
            FormatTags(response, sb);
            return sb.ToString();
        }

        private void FormatCollections(Application.Features.Stats.Response response, StringBuilder sb)
        {
            if (response.Collections.Any())
            {
                // sb.AppendLine(Formatter.Format(response.Collections));
            }
        }

        private void FormatTags(Application.Features.Stats.Response response, StringBuilder sb)
        {
            if (response.Tags.Any())
            {
               // sb.AppendLine(Formatter.Format(response.Tags));
            }

        }

        private void FormatTotals(Application.Features.Stats.Response response, StringBuilder sb)
        {
            // sb.AppendLine(Formatter.Format(new List<TotalsResponse>
            // {
            //     response.Totals
            // }));
        }
    }
}
