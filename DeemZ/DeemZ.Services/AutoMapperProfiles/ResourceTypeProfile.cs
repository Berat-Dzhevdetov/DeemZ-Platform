namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Resources;

    public class ResourceTypeProfile : Profile
    {
        public ResourceTypeProfile()
        {
            CreateMap<Resource, DetailsResourseViewModel>()
                .ForMember(x => x.ResourceType, o => o.MapFrom(src => src.ResourceType.Name));
        }
    }
}
