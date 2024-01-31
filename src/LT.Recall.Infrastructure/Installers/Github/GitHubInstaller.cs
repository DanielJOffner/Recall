using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.IO;
using LT.Recall.Infrastructure.Properties;

namespace LT.Recall.Infrastructure.Installers.Github
{
    public class GitHubInstaller : IInstaller
    {
        public string Description => "Install collections from the Recall Github repo";
        public string Name => "Github Installer";

        private readonly GitHubClient _gitHubClient;
        private readonly TempFileInstaller _tempFileInstaller;

        public GitHubInstaller(GitHubClient gitHubClient, TempFileInstaller tempFileInstaller)
        {
            _gitHubClient = gitHubClient;
            _tempFileInstaller = tempFileInstaller;
        }


        public async Task<(int inserted, int updated)> Install(string collectionOrLocation)
        {
            if (!await _gitHubClient.Exists(collectionOrLocation))
                throw new InfrastructureError(string.Format(Resources.GitHubCollectionNotFoundError, collectionOrLocation), InfraErrorCode.GitHubCollectionNotFound);

            using var writer = new TempFileWriter();
            var contents = await _gitHubClient.GetContents(collectionOrLocation);
            var tempFile = writer.Write(contents.contents, contents.extension);

            return await _tempFileInstaller.Install(tempFile);
        }

        public Task<List<string>> ListCollections()
        {
            return _gitHubClient.ListCollections();
        }
    }
}
