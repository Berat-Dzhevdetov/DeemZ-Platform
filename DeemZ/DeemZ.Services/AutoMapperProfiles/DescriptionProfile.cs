namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Description;

    public class DescriptionProfile : Profile
    {
        public DescriptionProfile()
        {
            CreateMap<Description, DetailsDescriptionViewModel>()
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name));
        }
    }
}
