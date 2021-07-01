using DeemZ.Data;

namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Answer
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Text { get; set; }

        [Required]
        public Question Question { get; set; }
        [Required]
        public string QuestionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
