namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.PromoCodes;

    public class PromoCodeProfile : Profile
    {
        public PromoCodeProfile()
        {
            CreateMap<PromoCode, PromoCodeDetailsViewModel>();
        }
    }
}
