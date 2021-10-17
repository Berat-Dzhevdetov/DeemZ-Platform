namespace DeemZ.Models.FormModels.ChatMessage
{
    using System.ComponentModel.DataAnnotations;
    public class ChatMessageInputModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public string CourseId { get; set; }
    }
}