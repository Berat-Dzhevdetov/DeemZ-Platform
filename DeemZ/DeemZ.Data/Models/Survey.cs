namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Survey : BaseModel
    {
        [Required]
        [MaxLength(DataConstants.Survey.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public string CourseId { get; set; }
        [Required]
        public Course Course { get; set; }

        public bool IsPublic { get; set; } = false;

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<SurveyQuestion> Questions { get; set; } = new HashSet<SurveyQuestion>();
        public ICollection<ApplicationUserSurvey> Users { get; set; } = new HashSet<ApplicationUserSurvey>();
    }
}