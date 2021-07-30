namespace DeemZ.Services.AutoMapperProfiles
{
    using AutoMapper;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Reports;
    using DeemZ.Models.ViewModels.Reports;

    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<AddReportFormModel, Report>()
                .ForMember(x => x.LectureId, o => o.MapFrom(src => src.Id));

            CreateMap<Report, ReportViewReport>();

            CreateMap<Report, PreviewReportViewModel>();
        }
    }
}