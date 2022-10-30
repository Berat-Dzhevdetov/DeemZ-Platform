namespace DeemZ.Services.EmailSender
{
    using System.Threading.Tasks;

    public interface IEmailSenderService
    {
        Task SendEmailById(string uid, string subject, string htmlContent);
        Task SendEmailAsync(
            string to,
            string subject,
            string htmlContent);

        Task SendEmailToUsers(string subject, string content, string[] users);
    }
}
