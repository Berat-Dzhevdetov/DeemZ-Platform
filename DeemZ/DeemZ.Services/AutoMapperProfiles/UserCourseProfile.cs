namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Administration;

    public class UserCourseProfile : Profile
    {
        public UserCourseProfile()
        {
            CreateMap<UserCourse, UserCoursesViewModel>()
                .ForMember(x => x.CourseName, o => o.MapFrom(src => src.Course.Name))
                .ForMember(x => x.UserName, o => o.MapFrom(src => src.User.UserName));

            CreateMap<UserCourse, CoursesViewModel>()
                .ForMember(x => x.Name,o => o.MapFrom(src => src.Course.Name))
                .ForMember(x => x.Credits,o => o.MapFrom(src => src.Course.Credits))
                .ForMember(x => x.StartDate, o => o.MapFrom(src => src.Course.StartDate))
                .ForMember(x => x.EndDate, o => o.MapFrom(src => src.Course.EndDate))
                .ForMember(x => x.Price, o => o.MapFrom(src => src.Course.Price))
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Course.Id));
        }
    }
}
