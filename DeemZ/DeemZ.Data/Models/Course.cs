namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Course;

    public class Course : BaseModel
    {
        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal Price { get; set; } = DefaultPrice;

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public int Credits { get; set; }

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string StartDateDescription { get; set; }
        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string LectureDescription { get; set; }
        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string ExamDescription { get; set; }

        public DateTime SignUpStartDate { get; set; } = DateTime.UtcNow;
        public DateTime SignUpEndDate { get; set; } = DateTime.UtcNow.AddDays(14);

        public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();
        public ICollection<Lecture> Lectures { get; set; } = new HashSet<Lecture>();
        public ICollection<Survey> Surveys { get; set; } = new HashSet<Survey>();
        public ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
        public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
    }
}