namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Models.ViewModels.Administration;
    using DeemZ.Models.ViewModels.Course;

    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, IndexCourseViewModel>()
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name))
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Id));

            CreateMap<Course, DetailsCourseViewModel>()
                .ForMember(x => x.Lectures, o => o.MapFrom(src => src.Lectures))
                .ForMember(x => x.Credits, o => o.MapFrom(src => src.Credits))
                .ForMember(x => x.SignUpStartDate, o => o.MapFrom(src => src.SignUpStartDate))
                .ForMember(x => x.SignUpEndDate, o => o.MapFrom(src => src.SignUpEndDate));

            CreateMap<Course, IndexSignUpForCourseViewModel>()
                .ForMember(x => x.SignUpStartDate, o => o.MapFrom(src => src.SignUpStartDate.ToString("dd/MM/yyyy")))
                .ForMember(x => x.SignUpEndDate, o => o.MapFrom(src => src.SignUpEndDate.ToString("dd/MM/yyyy")));

            CreateMap<Course, SignUpCourseFormModel>()
                .ForMember(x => x.CourseName, o => o.MapFrom(src => src.Name));

            CreateMap<Course, CoursesViewModel>()
                .ForMember(x => x.StartDate, o => o.MapFrom(src => src.StartDate))
                .ForMember(x => x.EndDate, o => o.MapFrom(src => src.EndDate));

            CreateMap<AddCourseFormModel, Course>();

        }
    }
}