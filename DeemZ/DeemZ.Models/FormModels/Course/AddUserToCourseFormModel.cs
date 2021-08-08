namespace DeemZ.Models.FormModels.Course
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class AddUserToCourseFormModel
    {
        [Required]
        [StringLength(User.MaxUsernameLength,
            ErrorMessage ="{0} should be between {2} and {1} letters",
            MinimumLength = User.MinUsernameLength)]
        public string Username { get; set; }

        [Display(Name = "Course")]
        public string CourseId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Courses { get; set; }

        [Display(Name = "Is paid")]
        public bool IsPaid { get; set; }
    }
}
