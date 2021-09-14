namespace DeemZ.Global.Extensions
{
    using System;
    public static class StringExtentions
    {
        public static string ReplaceAll(this string word,char oldChar,char newChar)
        {
            while (word.Contains(oldChar))
                word = word.Replace(oldChar,newChar);

            return word;
        }

        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
                return input.Substring(0, input.Length - suffixToRemove.Length);

            return input;
        }
    }
}