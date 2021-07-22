namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Description;
    using DeemZ.Models.ViewModels.Description;

    public class DescriptionProfile : Profile
    {
        public DescriptionProfile()
        {
            CreateMap<Description, DetailsDescriptionViewModel>()
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name));

            CreateMap<AddDescriptionFormModel, Description>();

            CreateMap<Description, EditDescriptionFormModel>()
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Id))
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name));
        }
    }
}