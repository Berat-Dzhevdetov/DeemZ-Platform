namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Exam;

    public class Exam : BaseModel
    {
        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Course Course { get; set; }
        [Required]
        public string CourseId { get; set; }

        public bool ShuffleQuestions { get; set; } = true;

        public bool ShuffleAnswers { get; set; } = true;

        [Required]
        [MaxLength(MaxPasswordLength)]
        public string Password { get; set; }

        public bool IsPublic { get; set; } = false;

        public ICollection<Question> Questions { get; set; } = new HashSet<Question>();

        public ICollection<ApplicationUserExam> Users { get; set; } = new HashSet<ApplicationUserExam>();
    }
}