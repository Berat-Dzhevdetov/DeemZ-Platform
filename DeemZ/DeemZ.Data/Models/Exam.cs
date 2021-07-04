namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Exam
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Course Course { get; set; }
        [Required]
        public string CourseId { get; set; }

        public int MaxPoints { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsPublic { get; set; } = false;

        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();

        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
    }
}
