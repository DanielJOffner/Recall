namespace LT.Recall.Application.Abstractions
{
    public interface IImportFileReaderFactory
    {
        IImportFileReader GetReader(string filePath);
    }
}
