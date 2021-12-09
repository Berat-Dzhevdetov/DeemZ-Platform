namespace DeemZ.Services.EmailSender
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Infrastructure;
    using DeemZ.Global.WebConstants;
    using DeemZ.Data;
    using System.Collections.Generic;

    public class EmailSenderService : IEmailSenderService
    {

        private readonly SendGridClient client;
        private readonly DeemZDbContext dbContext;

        public EmailSenderService(DeemZDbContext dbContext)
        {
            client = new SendGridClient(Secret.SendGrid.ApiKey);
            this.dbContext = dbContext;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var fromAddress = new EmailAddress(Constant.EmailSender.Email, Constant.EmailSender.Name);
            var toAddress = new EmailAddress(to);

            var html = GetTemplate(htmlContent);

            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, html);

            await client.SendEmailAsync(message);
        }

        public async Task SendEmailToUsers(string subject, string content, string[] users)
        {
            var recievers = (users.Where(x => x != null).Count() == 0) ? dbContext.Users.Select(x => x.Email).ToArray() : users;

            var tasks = new List<Task>();

            foreach (var user in recievers)
            {
                tasks.Add(SendEmailAsync(user, subject, content));
            }

            await Task.WhenAll(tasks);
        }

        private static string GetTemplate(string content)
            => @$"<div style='background-color:#f1f1f1;padding: 10px 20px;'>
<div style='color:#E9806E;font-weight:bold;font-size:32px;text-align:center;'>{Constant.EmailSender.Name}</div>
<div>{content}</div>
<div style='margin-top:25px;font-style:italic;font-size:12px;'>
    Best regards,<br>
    <span style='color:#E9806E'>{Constant.EmailSender.Name}</span>
    </div>
</div>";
    }
}
