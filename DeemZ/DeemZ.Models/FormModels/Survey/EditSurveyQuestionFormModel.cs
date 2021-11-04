namespace DeemZ.Models.FormModels.Survey
{
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.SurveyQuestion;

    public class EditSurveyQuestionFormModel
    {
        [Required]
        [StringLength(MaxQuestionLength, MinimumLength = MinNameLength)]
        public string Question { get; set; }

        [Display(Name = "Is Optional")]
        public bool IsOptional { get; set; } = false;
    }
}