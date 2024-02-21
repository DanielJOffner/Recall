using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Verbs.Base;
using MediatR;

namespace LT.Recall.Cli.Verbs
{
    internal class Export : Verb
    {
        public class ExportOptions : Program.Options
        {
            public string Path { get; set; } = string.Empty;
        }

        public Export(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<CliResult> ExecuteInner(List<string> args, IOptions options)
        {
            var response = await Mediator.Send(new Application.Features.Export.Request()
            {
                FilePath = ((ExportOptions)options).Path
            });
            return new CliResult(response.UserFriendlyMessage, ResultType.Success, response);
        }
    }
}
