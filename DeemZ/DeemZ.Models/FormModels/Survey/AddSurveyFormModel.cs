namespace DeemZ.Models.FormModels.Survey
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.Survey;

    public class AddSurveyFormModel
    {
        [Required]
        [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
        public string Name { get; set; }
        [Display(Name = "Is Public")]
        public bool IsPublic { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Now;
        public string CourseName { get; set; }
    }
}