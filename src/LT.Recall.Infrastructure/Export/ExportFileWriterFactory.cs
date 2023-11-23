using LT.Recall.Application.Abstractions;

namespace LT.Recall.Infrastructure.Export
{
    public class ExportFileWriterFactory : IExportFileWriterFactory
    {
        public IExportFileWriter GetWriter(string filePath)
        {
            return new ExportCsvFileWriter();
        }
    }
}
