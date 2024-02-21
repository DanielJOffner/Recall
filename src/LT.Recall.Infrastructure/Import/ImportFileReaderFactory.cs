using LT.Recall.Application.Abstractions;
using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.Properties;

namespace LT.Recall.Infrastructure.Import
{
    public class ImportFileReaderFactory : IImportFileReaderFactory
    {
        public IImportFileReader GetReader(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if(extension.Equals(".csv", StringComparison.InvariantCultureIgnoreCase))
                return new ImportCsvFileReader();

            throw new InfrastructureError(string.Format(Resources.UnsupportedFileTypeError, extension),
                InfraErrorCode.UnsupportedFileType);
        }
    }
}
