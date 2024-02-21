using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure.Installers.Github;
using LT.Recall.Infrastructure.Installers.Url;

namespace LT.Recall.Infrastructure.Installers
{
    public class InstallerFactory : IInstallerFactory
    {
        private readonly GitHubInstaller _gitHubInstaller;
        private readonly UrlInstaller _urlInstaller;

        public InstallerFactory(GitHubInstaller gitHubInstaller, UrlInstaller urlInstaller)
        {
            _gitHubInstaller = gitHubInstaller;
            _urlInstaller = urlInstaller;
        }

        public IInstaller GetInstaller(string collectionOrLocation)
        {
            if (IsUrl(collectionOrLocation))
            {
                return _urlInstaller;
            }

            return _gitHubInstaller;
        }

        private bool IsUrl(string collectionOrLocation)
        {
            return collectionOrLocation.StartsWith("http", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<IInstaller> ListInstallers()
        {
            return new List<IInstaller>()
            {
                _gitHubInstaller
            };
        }
    }
}
