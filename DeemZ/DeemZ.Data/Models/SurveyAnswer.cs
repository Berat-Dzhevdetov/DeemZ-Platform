namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SurveyAnswer
    {
        [Key]
        [Required]
        [MaxLength(DataConstants.DefaultIdLength)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DataConstants.SurveyAnswer.MaxTextLength)]
        public string Text { get; set; }

    }
}