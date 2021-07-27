namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Resource;
    using DeemZ.Models.ViewModels.Resources;

    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<Resource, DetailsResourseViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Id))
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name));

            CreateMap<Resource, IndexResourceViewModel>()
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Id))
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name));

            CreateMap<ResourceType, ResourceTypeFormModel>();

            CreateMap<AddResourceFormModel, Resource>();
        }
    }
}
