namespace DeemZ.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Forum : BaseModel
    {
        [Required]
        [MaxLength(DataConstants.Forum.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}