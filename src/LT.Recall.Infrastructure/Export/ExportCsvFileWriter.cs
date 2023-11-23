using CsvHelper;
using LT.Recall.Application.Abstractions;
using LT.Recall.Domain.Entities;
using LT.Recall.Infrastructure.Errors;
using LT.Recall.Infrastructure.Errors.Codes;
using LT.Recall.Infrastructure.Formats;
using System.Globalization;

namespace LT.Recall.Infrastructure.Export
{
    internal class ExportCsvFileWriter : IExportFileWriter
    {
        public int Write(string filePath, List<Command> commands)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(commands.Select(c => new CsvFileFormat
                    {
                        Id = c.CommandId,
                        CommandText = c.CommandText,
                        Description = c.Description,
                        Tags = string.Join(",", c.Tags.Select(x => x.Name)),
                        Collection = c.Collection
                    }));
                }
                return commands.Count;
            }
            catch (Exception e)
            {
                throw new InfrastructureError("", InfraErrorCode.UnknownExportFailure, e);
            }
        }
    }
}
