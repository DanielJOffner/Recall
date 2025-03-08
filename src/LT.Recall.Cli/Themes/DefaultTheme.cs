namespace LT.Recall.Cli.Themes
{
    public class DefaultTheme : ITheme
    {
        public ConsoleColor Error { get; } = ConsoleColor.Red;
        public ConsoleColor Warning { get; } = ConsoleColor.Yellow;
        public ConsoleColor Success { get; } = ConsoleColor.Green;
        public ConsoleColor Message { get; } = ConsoleColor.White;
        public ConsoleColor SelectionBackground { get; } = ConsoleColor.Gray;
        public ConsoleColor SelectionForeground { get; } = ConsoleColor.Black;
        public ConsoleColor CommandText { get; } = ConsoleColor.White;
        public ConsoleColor Divider => ConsoleColor.DarkGray;
        public ConsoleColor Description { get; } = ConsoleColor.Yellow;
        public ConsoleColor Collection { get; } = ConsoleColor.Cyan;
        public ConsoleColor Tags { get; } = ConsoleColor.Blue;
        public ConsoleColor Selection { get; } = ConsoleColor.DarkYellow;
    }
}
