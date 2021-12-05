namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Partners;

    public class PartnerProfile : Profile
    {
        public PartnerProfile()
        {
            CreateMap<Partner, PartnersDetailsViewModel>();
        }
    }
}
