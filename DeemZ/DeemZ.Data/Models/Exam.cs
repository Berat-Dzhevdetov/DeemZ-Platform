namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey(nameof(Course))]
        public string CourseId { get; set; }

        public int MaxPoints { get; set; }

        public ICollection<Question> Question { get; set; } = new HashSet<Question>();
    }
}
