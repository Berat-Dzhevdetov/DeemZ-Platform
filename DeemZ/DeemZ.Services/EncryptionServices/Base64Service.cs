namespace DeemZ.Services.EncryptionServices
{
    using System;
    using System.Text;
    public static class Base64Service
    {
        public static string Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(string encodedText)
        {
            var encodedBytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(encodedBytes);
        }
    }
}
