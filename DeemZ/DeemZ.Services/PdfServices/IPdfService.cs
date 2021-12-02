namespace DeemZ.Services.PdfServices
{
    using System.Threading.Tasks;

    public interface IPdfService
    {
        Task<bool> GenerateCertificate(double grade, string cid, string uid, string serverLink);
    }
}