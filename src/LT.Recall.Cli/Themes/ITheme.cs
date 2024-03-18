namespace LT.Recall.Cli.Themes
{
    public interface ITheme
    {
        ConsoleColor Error { get; }
        ConsoleColor Warning { get; }
        ConsoleColor Success { get; }
        ConsoleColor Message { get; }
        ConsoleColor SelectionBackground { get; }
        ConsoleColor SelectionForeground { get; }
        ConsoleColor CommandText { get; }
        ConsoleColor Divider { get; }
        ConsoleColor Description { get; }
        ConsoleColor Collection { get; }
        ConsoleColor Tags { get; }
        ConsoleColor Selection { get; }
    }
}
