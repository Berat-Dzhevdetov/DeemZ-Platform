namespace DeemZ.Models.FormModels.Forum
{
    using DeemZ.Data;
    using System.ComponentModel.DataAnnotations;

    public class AddCommentFormModel
    {
        [Required]
        [Display(Name = "Comment")]
        [StringLength(10000,
                ErrorMessage = "{0} should be at least {2} letters",
                MinimumLength = DataConstants.Comment.MinTextLength)]
        public string Text { get; set; }
    }
}