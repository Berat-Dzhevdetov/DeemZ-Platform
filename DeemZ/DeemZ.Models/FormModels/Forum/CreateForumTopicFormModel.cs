namespace DeemZ.Models.FormModels.Forum
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Data;

    public class CreateForumTopicFormModel
    {
        [Required]
        [StringLength(
            DataConstants.Forum.MaxTitleLength,
            ErrorMessage = "{0} should be between {2} and {1} letters",
            MinimumLength = DataConstants.Forum.MinTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(
            int.MaxValue,
            ErrorMessage = "{0} should be between at least {2} letters",
            MinimumLength = DataConstants.Forum.MinDescriptionLength)]
        public string Description { get; set; }

        public string UserId { get; set; }
    }
}
