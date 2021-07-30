namespace DeemZ.Models.ViewModels.Reports
{
    using System;

    public class ReportViewReport
    {
        public string Id { get; set; }
        public string UserUsername { get; set; }
        public string IssueDescription { get; set; }
        public string LectureName { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}