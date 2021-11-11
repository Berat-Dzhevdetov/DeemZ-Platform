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
    using DeemZ.Global.Extensions;
    using System.Threading.Tasks;

    public class ReportService : IReportService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ReportService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<string> AddReport(AddReportFormModel model,string uid)
        {
            var report = mapper.Map<Report>(model);

            report.UserId = uid;

            context.Reports.Add(report);
            await context.SaveChangesAsync();

            return report.Id;
        }

        public T GetReportById<T>(string id)
        {
            var report = context.Reports
                .Include(x => x.Lecture)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);

            return mapper.Map<T>(report);
        }

        public bool GetReportById(string id)
            => context.Reports.Any(x => x.Id == id);

        public async Task Delete(string id)
        {
            var report = GetReportById<Report>(id);

            context.Reports.Remove(report);
            await context.SaveChangesAsync();
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
