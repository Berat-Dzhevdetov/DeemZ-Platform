namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Models.ViewModels.Lectures;

    public class LectureProfile : Profile
    {
        public LectureProfile()
        {
            CreateMap<Lecture, DetailsLectureViewModel>()
                .ForMember(x => x.Resourses, o => o.MapFrom(src => src.Resources));

            CreateMap<Lecture, AddReportFormModel>();

            CreateMap<Lecture, LectureBasicInformationViewModel>()
                .ForMember(x => x.Name, o => o.MapFrom(src => src.Name))
                .ForMember(x => x.Id, o => o.MapFrom(src => src.Id));

            CreateMap<AddLectureFormModel, Lecture>();
        }
    }
}
