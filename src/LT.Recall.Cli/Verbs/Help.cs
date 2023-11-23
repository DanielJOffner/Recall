using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Verbs.Base;
using MediatR;

namespace LT.Recall.Cli.Verbs
{
    internal class Help : Verb
    {
        public class HelpOptions : Program.Options { }

        public Help(IMediator mediator) : base(mediator)
        {

        }

        protected override Task<CliResult> ExecuteInner(List<string> args, IOptions options)
        {
            return Task.FromResult(new CliResult(HelpTextGenerator.GenerateHelpText<Program.Options>()));
        }
    }
}
