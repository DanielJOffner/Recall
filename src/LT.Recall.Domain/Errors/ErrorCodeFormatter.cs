using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
[assembly: InternalsVisibleTo("LT.Recall.IntegrationTests")]

namespace LT.Recall.Domain.Errors
{
    internal static class ErrorCodeFormatter
    {
        private static Regex ErrorTypeRegex => new Regex(@"[A-Z]", RegexOptions.Compiled);

        /// <summary>
        /// InfrastructureErrorCode, 4 => IEC004
        /// </summary>
        public static string FormatErrorCode(Type error, int errorCode)
        {
            string errorTypeId = string.Join("", ErrorTypeRegex.Matches(error.Name).Select(x => x.Value));
            return $"{errorTypeId}{errorCode:D3}";
        }
    }
}
