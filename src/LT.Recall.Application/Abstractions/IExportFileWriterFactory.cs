namespace LT.Recall.Application.Abstractions
{
    public interface IExportFileWriterFactory 
    {
        public IExportFileWriter GetWriter(string filePath);
    }
}
