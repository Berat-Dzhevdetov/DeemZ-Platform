namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Survey
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DataConstants.Survey.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public string CourseId { get; set; }
        [Required]
        public Course Course { get; set; }

        public bool IsPublic { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
        public ICollection<SurveyQuestion> Questions { get; set; } = new HashSet<SurveyQuestion>();
    }
}
