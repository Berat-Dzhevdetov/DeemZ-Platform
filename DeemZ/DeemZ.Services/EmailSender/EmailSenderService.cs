namespace DeemZ.Services.EmailSender
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Threading.Tasks;
    using DeemZ.Infrastructure;
    using DeemZ.Global.WebConstants;
    using DeemZ.Models.ViewModels.User;
    using System.Collections.Generic;
    using DeemZ.Services.UserServices;

    public class EmailSenderService : IEmailSenderService
    {

        private readonly SendGridClient client;
        private readonly IUserService userService;

        public EmailSenderService(IUserService userService)
        {
            this.client = new SendGridClient(Secret.SendGrid.ApiKey);
            this.userService = userService;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var fromAddress = new EmailAddress(Constant.EmailSender.Email, Constant.EmailSender.Name);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);

            var response = await this.client.SendEmailAsync(message);
        }

        public async Task SendEmailToAllUsers(string subject, string content)
        {
            var users = userService.GetAllUsers<BasicUserInformationViewModel>();
            var Tos = new List<EmailAddress>();
            foreach (var user in users)
            {
                Tos.Add(new EmailAddress(user.Email));
            }

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Constant.EmailSender.Email, Constant.EmailSender.Name),
                Subject = subject,
                HtmlContent = content,
                Personalizations = new List<Personalization>
                {
                     new Personalization { Tos = Tos }
                }
            };

            await client.SendEmailAsync(msg);
        }

        public async Task SendEmailToSelectedUsers(string subject, string content, IEnumerable<BasicUserInformationViewModel> users)
        {
            var Tos = new List<EmailAddress>();
            foreach (var user in users)
            {
                Tos.Add(new EmailAddress(user.Email));
            }

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Constant.EmailSender.Email, Constant.EmailSender.Name),
                Subject = subject,
                HtmlContent = content,
                Personalizations = new List<Personalization>
                {
                     new Personalization { Tos = Tos }
                }
            };

            await client.SendEmailAsync(msg);
        }
    }
}
