namespace DeemZ.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SurveyAnswer : BaseModel
    {
        [Required]
        [MaxLength(DataConstants.SurveyAnswer.MaxTextLength)]
        public string Text { get; set; }

    }
}