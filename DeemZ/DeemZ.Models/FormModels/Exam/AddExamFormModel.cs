namespace DeemZ.Models.FormModels.Exam
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.Exam;

    public class AddExamFormModel
    {
        [Required]
        [StringLength(MaxNameLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = MinNameLength)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Shuffle questions")]
        public bool ShuffleQuestions { get; set; } = true;

        [Display(Name = "Shuffle answers")]
        public bool ShuffleAnswers { get; set; } = true;

        [Required]
        [StringLength(MaxPasswordLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = MinPasswordLength)]
        public string Password { get; set; }

        [Display(Name = "Is public")]
        public bool IsPublic { get; set; } = false;
    }
}