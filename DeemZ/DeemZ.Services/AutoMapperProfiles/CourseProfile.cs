namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Course;

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, IndexCourseViewModel>()
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name))
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Id));
        }
    }
}
