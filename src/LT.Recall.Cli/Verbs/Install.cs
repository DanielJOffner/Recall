using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Verbs.Base;
using MediatR;
using System.Text;

namespace LT.Recall.Cli.Verbs
{
    internal class Install : Verb
    {
        public class InstallOptions : Program.Options
        {
            public bool ListAll { get; set; }
        }

        public Install(IMediator mediator) : base(mediator)
        {
        }

        protected override async Task<CliResult> ExecuteInner(List<string> args, IOptions options)
        {
            var o = ((InstallOptions)options);

            if (o.ListAll)
            {
                await HandleListInstallers();
            }

            return await HandleInstall(args);
        }

        private async Task HandleListInstallers()
        {
            var sb = new StringBuilder();
            var result = await Mediator.Send(new Application.Features.ListInstallers.Request());
            foreach (var installer in result.Installers)
            {
                sb.AppendLine($"{installer.Name}");
                foreach (var collection in installer.Collections)
                {
                    sb.AppendLine($"  {collection}");
                }
            }
        }

        private async Task<CliResult> HandleInstall(List<string> args)
        {
            var result = await Mediator.Send(new Application.Features.Install.Request
            {
                CollectionOrLocation = args.FirstOrDefault() ?? string.Empty
            });

            return new CliResult(result.UserFriendlyMessage, ResultType.Success, result);
        }
    }
}
