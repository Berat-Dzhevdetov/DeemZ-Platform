namespace DeemZ.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Global.Attributes;

    using static DataConstants.Question;

    public class Question : BaseModel
    {
        [Required]
        [MaxLength(MaxTextLength)]
        public string Text { get; set; }

        public int Points { get; set; }

        public Exam Exam { get; set; }
        public string ExamId { get; set; }

        [EnsureCountElement(AtLeastCountAnswers)]
        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
    }
}