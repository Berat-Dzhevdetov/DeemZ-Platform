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

        public int Credits { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();
        public ICollection<Lecture> Lectures { get; set; } = new HashSet<Lecture>();

        public decimal Price { get; set; } = DataConstants.Course.DefaultPrice;
    }
}