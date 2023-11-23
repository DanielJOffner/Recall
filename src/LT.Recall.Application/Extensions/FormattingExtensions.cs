namespace LT.Recall.Application.Extensions
{
    public static class FormattingExtensions
    {
        public static string ToBytesDisplayValue(this long size)
        {
            if (size < 1024)
            {
                return $"{size}B";
            }
            if (size < 1024 * 1024)
            {
                return $"{size / 1024}KB";
            }
            if (size < 1024 * 1024 * 1024)
            {
                return $"{size / (1024 * 1024)}MB";
            }
            return $"{size / (1024 * 1024 * 1024)}GB";
        }
    }
}
