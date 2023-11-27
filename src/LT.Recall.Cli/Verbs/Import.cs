using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Verbs.Base;
using MediatR;

namespace LT.Recall.Cli.Verbs
{
    internal class Import : Verb
    {
        public class ImportOptions : Program.Options
        {
            public string Path { get; set; } = string.Empty;
        }

        public Import(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<CliResult> ExecuteInner(List<string> args, IOptions options)
        {
            var response = await Mediator.Send(new Application.Features.Import.Request
            {
                FilePath = ((ImportOptions)options).Path
            });

            return new CliResult(response.UserFriendlyMessage, ResultType.Success, response);
        }
    }
}
