namespace DeemZ.Services
{
    using CloudinaryDotNet;

    public static class Secret
    {
        public class CloudinarySetup
        {
            private const string cloudName = "dlquhyz0t";
            private const string key = "797481621562733";
            private const string secretKey = "c6QXlIqUFFcUzPzYdm3QCtfg1f8";
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