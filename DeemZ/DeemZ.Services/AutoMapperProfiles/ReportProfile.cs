namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Reports;

    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<AddReportFormModel, Report>()
                .ForMember(x => x.LectureId, o => o.MapFrom(src => src.Id));
        }
    }
}
