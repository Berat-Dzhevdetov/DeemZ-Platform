﻿namespace DeemZ.Models.FormModels.Course
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DeemZ.Data.DataConstants.Course;

    public class AddCourseFormModel
    {
        [Required]
        [StringLength(MaxNameLength,
            ErrorMessage ="{0} should be between {2} and {1} letters",
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

        [Range(MinCredits,MaxCredits)]
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

        public bool Redirect { get; set; }
        public bool BasicLectures { get; set; }

        [Required]
        [StringLength(MaxDescriptionLength,
                MinimumLength = MinDescriptionLength)]
        [Display(Name = "Suitable For Description")]
        public string SuitableForDescription { get; set; }
        [Required]
        [StringLength(MaxDescriptionLength,
                MinimumLength = MinDescriptionLength)]
        [Display(Name = "Start Date Description")]
        public string StartDateDescription { get; set; }
        [Required]
        [StringLength(MaxDescriptionLength,
                MinimumLength = MinDescriptionLength)]
        [Display(Name = "Lecture Description")]
        public string LectureDescription { get; set; }
        [Required]
        [StringLength(MaxDescriptionLength,
                MinimumLength = MinDescriptionLength)]
        [Display(Name = "Exam Description")]
        public string ExamDescription { get; set; }
    }
}