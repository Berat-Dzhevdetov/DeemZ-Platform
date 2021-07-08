namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
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
        public decimal Price { get; set; } = DataConstants.Course.DefaultPrice;

        public DateTime SignUpStartDate { get; set; } = DateTime.UtcNow;
        public DateTime SignUpEndDate { get; set; } = DateTime.UtcNow.AddDays(14);

        public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();
        public ICollection<Lecture> Lectures { get; set; } = new HashSet<Lecture>();
    }
}