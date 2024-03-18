using LT.Recall.Application.Abstractions;
using LT.Recall.Domain.Entities;
using LT.Recall.Domain.ValueObjects;
using System.Linq;
using System.Text.RegularExpressions;

namespace LT.Recall.Infrastructure.Import
{
    internal class ImportCsvFileReader : IImportFileReader
    {
        public List<Command> Read(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            try
            {
                var lines = File.ReadAllLines(filePath);
                var headers = lines[0].Split(','); // Get headers

                var idIndex = Array.IndexOf(headers, nameof(Command.Id));
                var commandTextIndex = Array.IndexOf(headers, nameof(Command.CommandText));
                var descriptionIndex = Array.IndexOf(headers, nameof(Command.Description));
                var tagsIndex = Array.IndexOf(headers, nameof(Command.Tags));
                var collectionIndex = Array.IndexOf(headers, nameof(Command.Collection));

                var commands = new List<Command>();

                foreach (var line in lines.Skip(1)) // Skip header line
                {
                    var fields = SplitCsvLine(line);

                    var tags = fields[tagsIndex].Split(',').Select(x => new Tag(TrimQuotes(x))).ToList();
                    var commandText = TrimQuotes(fields[commandTextIndex]);
                    var description = TrimQuotes(fields[descriptionIndex]);
                    var commandId = int.Parse(fields[idIndex]);
                    var collection = TrimQuotes(fields[collectionIndex]);

                    var command = new Command(commandText, description, tags, commandId: commandId, collection: collection);
                    commands.Add(command);
                }

                return commands;
            }
            catch (Exception e)
            {
                throw new Exception($"Invalid CSV format in file: {filePath}", e);
            }
        }

        private string TrimQuotes(string value)
        {
            return value.Trim('"');
        }

        private static string[] SplitCsvLine(string line)
        {
            var pattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
            var regex = new Regex(pattern);
            return regex.Split(line);
        }
    }
}
