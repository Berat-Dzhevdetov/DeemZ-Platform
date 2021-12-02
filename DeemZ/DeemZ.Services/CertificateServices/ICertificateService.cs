namespace DeemZ.Services.CertificateServices
{
    public interface ICertificateService
    {
        T GetCertificateById<T>(string cid);
        T GetCertificateByExternalNumber<T>(int externalNumber);
    }
}