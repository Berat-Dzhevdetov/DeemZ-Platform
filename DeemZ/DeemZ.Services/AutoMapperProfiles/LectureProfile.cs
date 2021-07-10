namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.ViewModels.Lectures;

    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            CreateMap<Lecture, DetailsLectureViewModel>()
                .ForMember(x => x.Resourses, o => o.MapFrom(src => src.Resources));
        }
    }
}
