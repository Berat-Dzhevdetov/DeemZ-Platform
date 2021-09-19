namespace DeemZ.Services.EmailSender
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Threading.Tasks;
    using DeemZ.Infrastructure;
    using DeemZ.Global.WebConstants;

    public class EmailSenderService : IEmailSenderService
    {

        private readonly SendGridClient client;

        public EmailSenderService()
        {
            this.client = new SendGridClient(Secret.SendGrid.ApiKey);
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var fromAddress = new EmailAddress(Constant.EmailSender.Email, Constant.EmailSender.Name);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);

            var response = await this.client.SendEmailAsync(message);
            //Console.WriteLine(response.StatusCode);
            //Console.WriteLine(await response.Body.ReadAsStringAsync());
        }
    }
}
