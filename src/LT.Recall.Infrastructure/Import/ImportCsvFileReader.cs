using CsvHelper;
using CsvHelper.Configuration;
using LT.Recall.Application.Abstractions;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;
using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.Formats;
using LT.Recall.Infrastructure.Properties;
using System.Globalization;

namespace LT.Recall.Infrastructure.Import
{
    internal class ImportCsvFileReader : IImportFileReader
    {
        private IReaderConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim
        };

        public List<Command> Read(string filePath)
        {
            if(!File.Exists(filePath))
                throw new NotFoundError(filePath, typeof(string));

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, configuration))
                {
                    var records = csv.GetRecords<CsvFileFormat>();
                    return records.Select(r => new Command(r.CommandText, r.Description, r.Tags.Split(',').Select(x => new Tag(x)).ToList(), commandId: r.Id, collection: r.Collection)).ToList();
                }
            }
            catch (Exception e)
            {
                throw new InfrastructureError(string.Format(Resources.InvalidCsvFormatError, filePath), InfraErrorCode.InvalidFileFormat, e);
            }
        }
    }
}
