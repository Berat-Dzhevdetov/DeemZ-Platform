namespace DeemZ.Web.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Web.Models.ViewModels.Course;
    using DeemZ.Web.Models.ViewModels.User;
    using System.Linq;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
        //    CreateMap<ApplicationUser, IndexUserViewModel>()
        //        .ForMember(x => x.Credits, o => o.MapFrom(src => src.Exams.Sum(x => x.Credits)))
        //        .ForMember(x => x.Courses, o => o.MapFrom(src => src.UserCourses.Select(x => new IndexCourseViewModel
        //        {
        //            Id = x.CourseId,
        //            Name = 
        //        })));
        }
    }
}
