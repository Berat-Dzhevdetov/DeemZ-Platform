namespace DeemZ.Services.ReportService
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
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

        public IEnumerable<T> GetReports<T>(int page = 1, int quantity = 20)
            => context.Reports
                .Include(x => x.Lecture)
                .Include(x => x.User)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();
    }
}
