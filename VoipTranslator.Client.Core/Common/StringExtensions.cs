namespace VoipTranslator.Client.Core.Common
{
    public static class StringExtensions
    {
        public static bool EndsWithAny(this string sourceString, params string[] substrings)
        {
            foreach (var substring in substrings)
            {
                if (sourceString.EndsWith(substring))
                    return true;
            }
            return false;
        }

        public static bool StartsWithAny(this string sourceString, params string[] substrings)
        {
            foreach (var substring in substrings)
            {
                if (sourceString.StartsWith(substring))
                    return true;
            }
            return false;
        }
    }
}
