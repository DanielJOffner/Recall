using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure.Installers.Github;
using LT.Recall.Infrastructure.Installers.Uri;

namespace LT.Recall.Infrastructure.Installers
{
    public class InstallerFactory : IInstallerFactory
    {
        private readonly GitHubInstaller _gitHubInstaller;
        private readonly UriInstaller _uriInstaller;

        public InstallerFactory(GitHubInstaller gitHubInstaller, UriInstaller uriInstaller)
        {
            _gitHubInstaller = gitHubInstaller;
            _uriInstaller = uriInstaller;
        }

        public IInstaller GetInstaller(string collectionOrLocation)
        {
            if (IsUri(collectionOrLocation))
            {
                return _uriInstaller;
            }

            return _gitHubInstaller;
        }

        private bool IsUri(string collectionOrLocation)
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
