namespace DeemZ.Models.ViewModels.ChatMessages
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class ChatMessageViewModel
    {
        [Required]
        public string Content { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserUsername { get; set; }

        public string ApplicationUserImgUrl { get; set; }

        public string CourseName { get; set; }

        //TODO: ADD COURSE PROP
        public string CourseId { get; set; }

        public DateTime SentOn { get; set; } = DateTime.UtcNow;
    }
}