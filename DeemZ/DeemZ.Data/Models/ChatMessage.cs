using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Data.Models
{
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
    }
}
