namespace DeemZ.Services.EmailSender
{
    using DeemZ.Models.ViewModels.User;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSenderService
    {
        Task SendEmailAsync(
            string to,
            string subject,
            string htmlContent);

        Task SendEmailToUsers(string subject, string content, string[] users);
    }
}
