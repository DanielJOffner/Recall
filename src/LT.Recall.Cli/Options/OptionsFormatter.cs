namespace LT.Recall.Cli.Options
{
    internal static class OptionsFormatter
    {
        private const string shortOptionPrefix = "-";
        private const string longOptionPrefix = "--";

        internal static bool IsOptionArg(string arg)
        {
            return arg.StartsWith(shortOptionPrefix) || arg.StartsWith(longOptionPrefix);
        }

        internal static string GetShortOption(string property)
        {
            return $"{shortOptionPrefix}{property.Substring(0, 1).ToLowerInvariant()}";
        }

        internal static string GetLongOption(string property)
        {
            return $"{longOptionPrefix}{property.ToLowerInvariant()}";
        }
    }
}
