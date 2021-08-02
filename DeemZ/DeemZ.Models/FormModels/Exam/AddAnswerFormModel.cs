namespace DeemZ.Models.FormModels.Exam
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Answer;

    public class AddAnswerFormModel
    {
        [StringLength(MaxTextLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = MinTextLength)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}