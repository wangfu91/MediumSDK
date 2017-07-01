using System.Text.RegularExpressions;

namespace MediumSDK.Utils
{
    internal static class Extensions
    {
        internal static string PascalCaseToCamelCase(this string source)
        {
            if (source.Length > 0 &&
                new Regex("([A-Z])").IsMatch(source[0].ToString()))
            {
                return source[0].ToString().ToLowerInvariant() + source.Substring(1);
            }
            return source;
        }

    }
}
