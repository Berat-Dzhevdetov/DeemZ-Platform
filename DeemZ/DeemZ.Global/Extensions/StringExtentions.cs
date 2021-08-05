namespace DeemZ.Global.Extensions
{
    public static class StringExtentions
    {
        public static string ReplaceAll(this string word,char oldChar,char newChar)
        {
            while (word.Contains(oldChar))
            {
                word = word.Replace(oldChar,newChar);
            }
            return word;
        }
    }
}