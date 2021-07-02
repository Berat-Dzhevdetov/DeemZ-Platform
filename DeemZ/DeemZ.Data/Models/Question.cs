namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Question
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Text { get; set; }

        public int Points { get; set; }

        public bool IsMultipleChoice { get; set; }

        public Exam Exam { get; set; }
        public string ExamId { get; set; }

        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
    }
}
