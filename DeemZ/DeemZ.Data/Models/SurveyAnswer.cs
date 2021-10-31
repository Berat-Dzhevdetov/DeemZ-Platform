namespace DeemZ.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SurveyAnswer : BaseModel
    {
        [Required]
        [MaxLength(DataConstants.SurveyAnswer.MaxTextLength)]
        public string Text { get; set; }

        public string SurveyQuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }

        public ICollection<ApplicationUserSurveyAnswer> Users { get; set; } = new HashSet<ApplicationUserSurveyAnswer>();

    }
}