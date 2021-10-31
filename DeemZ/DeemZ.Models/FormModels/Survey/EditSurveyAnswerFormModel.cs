namespace DeemZ.Models.FormModels.Survey
{
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Data;

    public class EditSurveyAnswerFormModel
    {
        [Required]
        [MaxLength(DataConstants.SurveyAnswer.MaxTextLength)]
        public string Text { get; set; }
    }
}
