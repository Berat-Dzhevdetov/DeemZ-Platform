using DeemZ.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Models.FormModels.ChatMessage
{
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
