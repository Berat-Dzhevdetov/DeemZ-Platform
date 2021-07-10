namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Resources;

    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<Resource, DetailsResourseViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Id))
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name))
                .ForMember(x => x.Path, o => o.MapFrom(src => src.Path))
                .ForMember(x => x.ResourceType, o => o.MapFrom(src => src.ResourceType));
        }
    }
}
