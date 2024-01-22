using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.IO;
using LT.Recall.Infrastructure.Properties;

namespace LT.Recall.Infrastructure.Installers.Github
{
    public class GitHubInstaller : IInstaller
    {
        private readonly GitHubClient _gitHubClient;
        private readonly IImportFileReaderFactory _importFileReaderFactory;
        private readonly ICommandRepository _commandRepository;
        public GitHubInstaller(GitHubClient gitHubClient, IImportFileReaderFactory importFileReaderFactory, ICommandRepository commandRepository)
        {
            _gitHubClient = gitHubClient;
            _importFileReaderFactory = importFileReaderFactory;
            _commandRepository = commandRepository;
        }

        public async Task<(int inserted, int updated)> Install(string collectionOrLocation)
        {
            if (!await _gitHubClient.Exists(collectionOrLocation))
                throw new InfrastructureError(string.Format(Resources.GitHubCollectionNotFoundError, collectionOrLocation), InfraErrorCode.GitHubCollectionNotFound);

            using var writer = new TempFileWriter();
            var contents = await _gitHubClient.GetContents(collectionOrLocation);
            var tempFile = writer.Write(contents.contents, contents.extension);
            var importFileReader = _importFileReaderFactory.GetReader(tempFile);
            var commands = importFileReader.Read(tempFile);
            var result = await _commandRepository.BulkUpsert(commands);

            return (result.inserted, result.updated);
        }

        public Task<List<string>> ListCollections()
        {
            return _gitHubClient.ListCollections();
        }
    }
}
