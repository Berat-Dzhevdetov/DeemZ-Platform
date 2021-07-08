namespace DeemZ.Web.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Web.Models.ViewModels.User;
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<ApplicationUser, IndexUserViewModel>()
            //    .ForMember(x => x.Courses, o => o.MapFrom(src => src.UserCourses))
            //    .ForMember(x => x.Credits, o => o.MapFrom(src => src.));
        }
    }
}
