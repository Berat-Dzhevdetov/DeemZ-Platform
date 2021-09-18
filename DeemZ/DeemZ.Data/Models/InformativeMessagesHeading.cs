namespace DeemZ.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.InformativeMessages;

    public class InformativeMessagesHeading : BaseModel
    {
        [Required]
        [MaxLength(MaxTitleLength)]
        public string Title { get; set; }
        public ICollection<InformativeMessage> InformativeMessages { get; set; } = new HashSet<InformativeMessage>();
    }
}