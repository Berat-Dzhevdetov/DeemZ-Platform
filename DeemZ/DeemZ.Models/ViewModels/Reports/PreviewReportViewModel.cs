namespace DeemZ.Models.ViewModels.Reports
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PreviewReportViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
        [Display(Name = "Username")]
        public string UserUserName { get; set; }

        [Display(Name = "Issue description")]
        public string IssueDescription { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Lecture name")]
        public string LectureName { get; set; }
        public string LectureCourseId { get; set; }
    }
}