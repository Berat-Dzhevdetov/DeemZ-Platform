namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class ChatMessage : BaseModel
    {
        [Required]
        public string Content { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public Course Course { get; set; }
        [Required]
        public string CourseId { get; set; }

        public DateTime SentOn { get; set; } = DateTime.UtcNow;

        public int Likes { set; get; } = 0;
    }
}