using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure.Installers.Github;

namespace LT.Recall.Infrastructure.Installers
{
    public class InstallerFactory : IInstallerFactory
    {
        private readonly GitHubInstaller _gitHubInstaller;

        public InstallerFactory(GitHubInstaller gitHubInstaller)
        {
            _gitHubInstaller = gitHubInstaller;
        }

        public IInstaller GetInstaller(string collectionOrLocation)
        {
            switch (collectionOrLocation)
            {
                default:
                    return _gitHubInstaller;
            }
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
