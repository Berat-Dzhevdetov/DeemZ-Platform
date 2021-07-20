namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment : BaseModel
    {
        [Required]
        public Forum Forum { get; set; }

        [Required]
        public string ForumId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}