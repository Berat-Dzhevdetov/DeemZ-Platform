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
    using DeemZ.Data;
    using System;

    public class EmailSenderService : IEmailSenderService
    {

        private readonly SendGridClient client;
        private readonly DeemZDbContext dbContext;

        public EmailSenderService(DeemZDbContext dbContext)
        {
            this.client = new SendGridClient(Secret.SendGrid.ApiKey);
            this.dbContext = dbContext;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var fromAddress = new EmailAddress(Constant.EmailSender.Email, Constant.EmailSender.Name);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);

            await this.client.SendEmailAsync(message);
        }

        public async Task SendEmailToUsers(string subject, string content, string[] users)
        {
            var recievers = (users.Where(x=>x!=null).Count() == 0) ? dbContext.Users.Select(x => x.Email).ToArray() : users;
            foreach (var user in recievers)
            {
                await SendEmailAsync(user, subject, content);
            }
        }
    }
}
