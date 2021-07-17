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
        }
    }
}
