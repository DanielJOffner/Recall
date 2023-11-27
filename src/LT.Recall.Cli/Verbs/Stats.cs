using AsciiTableFormatter;
using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Verbs.Base;
using MediatR;
using System.Text;
using static LT.Recall.Application.Features.Stats.Response;

namespace LT.Recall.Cli.Verbs
{
    internal class Stats : Verb
    {
        public class StatsOptions : Program.Options { }
        public Stats(IMediator mediator) : base(mediator)
        {

        }

        protected override async Task<CliResult> ExecuteInner(List<string> args, IOptions options)
        {

            var response = await Mediator.Send(new Application.Features.Stats.Request());
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
                sb.AppendLine(Formatter.Format(response.Collections));
            }
        }

        private void FormatTags(Application.Features.Stats.Response response, StringBuilder sb)
        {
            if (response.Tags.Any())
            {
                sb.AppendLine(Formatter.Format(response.Tags));
            }

        }

        private void FormatTotals(Application.Features.Stats.Response response, StringBuilder sb)
        {
            sb.AppendLine(Formatter.Format(new List<TotalsResponse>
            {
                response.Totals
            }));
        }
    }
}
