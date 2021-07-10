namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SurveyQuestion
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DataConstants.SurveyQuestion.MaxQuestionLength)]
        public string Question { get; set; }

        [Required]
        public string SurveyId { get; set; }
        [Required]
        public Survey Survey { get; set; }

        public ICollection<SurveyAnswer> Answers { get; set; } = new HashSet<SurveyAnswer>();
    }
}