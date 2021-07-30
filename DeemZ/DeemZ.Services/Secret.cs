namespace DeemZ.Services
{
    using CloudinaryDotNet;

    public static class Secret
    {
        public class CloudinarySetup
        {
            private const string cloudName = "";
            private const string key = "";
            private const string secretKey = "";
            private Account account;

            public CloudinarySetup()
            {
                this.Account = new Account(cloudName, key, secretKey);
            }

            public Account Account
            {
                get
                {
                    return account;
                }
                protected set
                {
                    this.account = value;
                }
            }
        }
    }
}