namespace DeemZ.Models.FormModels.Reports
{
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Data;

    public class AddReportFormModel
    {
        [Required]
        [Display(Name = "Description")]
        [StringLength(10000,
            ErrorMessage = "{0} should be at least {2} letters",
            MinimumLength = DataConstants.Report.MinDescriptionLength)]
        public string IssueDescription { get; set; }

        //Lecture Id
        [Required]
        public string LectureId { get; set; }
    }
}
