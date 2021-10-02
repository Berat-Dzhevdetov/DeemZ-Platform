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
    using System.Linq;

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

        public async Task SendEmailToUsers(string subject, string content, string[] users)
        {
            var recievers = (users.Where(x=>x!=null).Count() == 0) ? userService.GetAllUsers<BasicUserInformationViewModel>().Select(x => x.Email).ToArray() : users;
            foreach (var user in recievers)
            {
                await SendEmailAsync(user, subject, content);
            }
        }
    }
}
