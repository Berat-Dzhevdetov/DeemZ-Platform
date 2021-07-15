namespace DeemZ.Services.ReportService
{
    using AutoMapper;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Reports;
    public class ReportService : IReportService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ReportService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddReport(AddReportFormModel model,string uid)
        {
            var report = mapper.Map<Report>(model);

            report.UserId = uid;

            context.Reports.Add(report);
            context.SaveChanges();
        }
    }
}
