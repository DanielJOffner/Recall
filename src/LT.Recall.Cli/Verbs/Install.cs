using System.Runtime.CompilerServices;
using LT.Recall.Cli.Options;
using LT.Recall.Cli.Output;
using LT.Recall.Cli.Verbs.Base;
using System.Text;
using LT.Recall.Application.Features;
using LT.Recall.Cli.Properties;
using System.Diagnostics.CodeAnalysis;

namespace LT.Recall.Cli.Verbs
{
    internal class Install : Verb<Install.Options>
    {
        private readonly Application.Features.Install.Handler _installHandler;
        private readonly Application.Features.ListInstallers.Handler _listInstallersHandler;

        public Install(Application.Features.Install.Handler installHandler, ListInstallers.Handler listInstallersHandler)
        {
            _installHandler = installHandler;
            _listInstallersHandler = listInstallersHandler;
        }

        public class Options : Program.Options
        {
            [method: DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(Options))]
            public Options()
            {

            }
            public bool ListAll { get; set; }
        }

        protected override string HelpText => Resources.InstallHelpText;

        protected override async Task<CliResult> ExecuteInner(List<string> args, Options options)
        {
            if (options.ListAll)
            {
                return await HandleListInstallers();
            }

            return await HandleInstall(args);
        }

        private async Task<CliResult> HandleListInstallers()
        {
            var sb = new StringBuilder();
            var result = await _listInstallersHandler.Handle(new Application.Features.ListInstallers.Request(), CancellationToken.None);
            sb.AppendLine();
            foreach (var installer in result.Installers)
            {
                sb.AppendLine($"{installer.Name}");
                foreach (var collection in installer.Collections)
                {
                    sb.AppendLine($"  {collection}");
                }
            }
            return new CliResult(sb.ToString(), ResultType.Message, result);
        }

        private async Task<CliResult> HandleInstall(List<string> args)
        {
            var result = await _installHandler.Handle(new Application.Features.Install.Request
            {
                CollectionOrLocation = args.FirstOrDefault() ?? string.Empty
            }, CancellationToken.None);

            return new CliResult(result.UserFriendlyMessage, ResultType.Success, result);
        }
    }
}
