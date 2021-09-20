namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.InformativeMessages;

    public class InformativeMessage : BaseModel
    {
        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }
        public DateTime ShowFrom { get; set; } = DateTime.UtcNow;
        public DateTime ShowTo { get; set; } = DateTime.UtcNow.AddDays(7);
        public string InformativeMessagesHeadingId { get; set; }
        public InformativeMessagesHeading InformativeMessagesHeading { get; set; }
    }
}