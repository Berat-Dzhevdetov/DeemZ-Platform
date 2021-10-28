namespace DeemZ.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SurveyQuestion : BaseModel
    {
        [Required]
        [MaxLength(DataConstants.SurveyQuestion.MaxQuestionLength)]
        public string Question { get; set; }

        [Required]
        public string SurveyId { get; set; }
        [Required]
        public Survey Survey { get; set; }

        public bool IsOptional { get; set; } = false;

        public bool IsOpenAnswer { get; set; } = false;

        public ICollection<SurveyAnswer> Answers { get; set; } = new HashSet<SurveyAnswer>();
    }
}