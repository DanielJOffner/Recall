using LT.Recall.Application.Abstractions;

namespace LT.Recall.Infrastructure.Installers
{
    public class TempFileInstaller
    {
        private readonly IImportFileReaderFactory _importFileReaderFactory;
        private readonly ICommandRepository _commandRepository;

        public TempFileInstaller(IImportFileReaderFactory importFileReaderFactory, ICommandRepository commandRepository)
        {
            _importFileReaderFactory = importFileReaderFactory;
            _commandRepository = commandRepository;
        }

        public async Task<(int inserted, int updated)> Install(string tempFilePath)
        {
            var importFileReader = _importFileReaderFactory.GetReader(tempFilePath);
            var commands = importFileReader.Read(tempFilePath);
            var result = await _commandRepository.BulkUpsert(commands);

            return (result.inserted, result.updated);
        }
    }
}
