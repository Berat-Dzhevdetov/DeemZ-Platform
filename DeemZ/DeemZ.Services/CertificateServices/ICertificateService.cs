namespace DeemZ.Services.CertificateServices
{
    using System.Collections.Generic;

    public interface ICertificateService
    {
        T GetCertificateById<T>(string cid);
        T GetCertificateByExternalNumber<T>(int externalNumber);
        IEnumerable<T> GetUserCertificates<T>(string userId);
    }
}