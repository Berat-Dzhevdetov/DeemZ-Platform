namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.PromoCode;
    using DeemZ.Models.ViewModels.PromoCodes;

    public class PromoCodeProfile : Profile
    {
        public PromoCodeProfile()
        {
            CreateMap<PromoCode, PromoCodeDetailsViewModel>();
            CreateMap<PromoCode, EditPromoCodeFormModel>();
        }
    }
}
