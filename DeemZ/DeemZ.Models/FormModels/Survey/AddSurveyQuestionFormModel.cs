﻿namespace DeemZ.Models.FormModels.Survey
{
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.SurveyQuestion;

    public class AddSurveyQuestionFormModel
    {
        [Required]
        [StringLength(MaxQuestionLength, MinimumLength = MinNameLength)]
        public string Question { get; set; }
    }
}
