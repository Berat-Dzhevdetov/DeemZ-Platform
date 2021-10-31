namespace DeemZ.Models.FormModels.Survey
{
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Data;
    public class AddSurveyAnswerFormModel
    {
        [Required]
        [MaxLength(DataConstants.SurveyAnswer.MaxTextLength)]
        public string Text { get; set; }
    }
}