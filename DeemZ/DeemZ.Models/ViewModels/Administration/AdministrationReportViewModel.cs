namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.Reports;

    public class AdministrationReportViewModel : PagingBaseModel
    {
        public IEnumerable<ReportViewReport> Reports { get; set; }
    }
}