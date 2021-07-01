namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

        public Exam Exam { get; set; }

        [ForeignKey(nameof(Exam))]
        public string ExamId { get; set; }

        public ICollection<Lecture> Lectures { get; set; } = new HashSet<Lecture>();

        public decimal Price { get; set; } = DataConstants.Course.DefaultPrice;
    }
}