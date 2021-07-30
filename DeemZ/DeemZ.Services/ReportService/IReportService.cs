namespace DeemZ.Services.ReportService
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Reports;

    public interface IReportService
    {
        void AddReport(AddReportFormModel model, string uid);
        IEnumerable<T> GetReports<T>(int page = 1, int quantity = 20);
        T GetReportById<T>(string id);
        bool GetReportById(string id);
        void Delete(string id);
    }
}