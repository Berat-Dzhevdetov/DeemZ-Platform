namespace DeemZ.Models.FormModels.Exam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Global.Attributes;

    using static Data.DataConstants.Question;

    public class AddQuestionFormModel
    {
        [Required]
        [StringLength(MaxTextLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = MinTextLength)]
        public string Text { get; set; }

        [Range(MinPoints, MaxPoints)]
        public int Points { get; set; }

        public IList<AddAnswerFormModel> Answers { get; set; } = new List<AddAnswerFormModel>();
    }
}