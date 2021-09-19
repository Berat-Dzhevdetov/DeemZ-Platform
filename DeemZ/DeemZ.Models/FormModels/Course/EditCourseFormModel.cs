namespace DeemZ.Models.FormModels.Course
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DeemZ.Data.DataConstants.Course;
    public class EditCourseFormModel
    {
        [Required]
        [StringLength(MaxNameLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = MinNameLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter date")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Range((double)MinMoney, (double)MaxMoney)]
        public decimal Price { get; set; } = DefaultPrice;

        [Range(MinCredits, MaxCredits)]
        public int Credits { get; set; }

        [Required]
        [Display(Name = "Start Sign Up Date")]
        public DateTime SignUpStartDate { get; set; } = DateTime.UtcNow;
        [Required]
        [Display(Name = "End Sign Up Date")]
        public DateTime SignUpEndDate { get; set; } = DateTime.UtcNow.AddDays(14);

        [Required]
        [StringLength(MaxDescriptionLength,
                MinimumLength = MinDescriptionLength)]
        public string Description { get; set; }
    }
}
