namespace DeemZ.Models.FormModels.Forum
{
    using DeemZ.Data;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddMainCommentFormModel
    {
        [Required]
        [Display(Name = "Comment")]
        [StringLength(10000,
                ErrorMessage = "{0} should be at least {2} letters",
                MinimumLength = DataConstants.Comment.MinTextLength)]
        public string Text { get; set; }
        public string Username { get; set; }
        public string UserProfileImg { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}