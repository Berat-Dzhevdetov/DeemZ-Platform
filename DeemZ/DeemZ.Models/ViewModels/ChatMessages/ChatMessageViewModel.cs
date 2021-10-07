using DeemZ.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DeemZ.Models.ViewModels.ChatMessages
{
    public class ChatMessageViewModel
    {
        [Required]
        public string Content { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        //TODO: ADD COURSE PROP
        public string CourseId { get; set; }

        public DateTime SentOn { get; set; } = DateTime.UtcNow;
    }
}
