namespace DeemZ.Services.ReportService
{
    using DeemZ.Models.FormModels.Reports;
    public interface IReportService
    {
        void AddReport(AddReportFormModel model, string uid);
    }
}