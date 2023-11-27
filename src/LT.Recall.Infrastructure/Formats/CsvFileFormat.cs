namespace LT.Recall.Infrastructure.Formats
{
    internal class CsvFileFormat
    {
        public int Id { get; set; } = 0;
        public string CommandText { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Tags { get; init; } = string.Empty;
        public string Collection { get; init; } = string.Empty;
    }
}
