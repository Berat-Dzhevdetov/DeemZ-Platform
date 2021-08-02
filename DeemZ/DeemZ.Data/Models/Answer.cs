namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Data;

    using static Data.DataConstants.Answer;

    public class Answer : BaseModel
    {

        [Required]
        [MaxLength(MaxTextLength)]
        public string Text { get; set; }

        [Required]
        public Question Question { get; set; }
        [Required]
        public string QuestionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}