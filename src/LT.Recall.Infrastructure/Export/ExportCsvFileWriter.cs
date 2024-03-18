using LT.Recall.Application.Abstractions;
using LT.Recall.Domain.Entities;

namespace LT.Recall.Infrastructure.Export
{
    internal class ExportCsvFileWriter : IExportFileWriter
    {
        public int Write(string filePath, List<Command> commands)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    // Write header
                    writer.WriteLine("Id,CommandText,Description,Tags,Collection");

                    foreach (var command in commands)
                    {
                        var line = $"{command.CommandId},{command.CommandText},{command.Description},{string.Join(",", command.Tags.Select(t => t.Name))},{command.Collection}";
                        writer.WriteLine(line);
                    }
                }

                return commands.Count;
            }
            catch (Exception e)
            {
                throw new Exception("Unknown export failure", e);
            }
        }
    }
}