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

        Task SendEmailToAllUsers(string subject, string content);

        Task SendEmailToSelectedUsers(string subject, string content, IEnumerable<BasicUserInformationViewModel> users);
    }
}
