namespace DeemZ.Services.EmailSender
{
    using System.Threading.Tasks;

    public interface IEmailSenderService
    {
        Task SendEmailAsync(
            string to,
            string subject,
            string htmlContent);
    }
}
