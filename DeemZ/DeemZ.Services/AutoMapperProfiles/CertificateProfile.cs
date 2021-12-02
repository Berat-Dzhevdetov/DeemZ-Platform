namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Certificates;

    public class CertificateProfile : Profile
    {
        public CertificateProfile()
        {
            CreateMap<Certificate, CertificateBasicViewModel>();
        }
    }
}
