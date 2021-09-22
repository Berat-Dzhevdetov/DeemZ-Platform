namespace DeemZ.Models.FormModels.InformativeMessages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using static DeemZ.Data.DataConstants.InformativeMessages;

    public class InformativeMessageFormModel
    {
        [Required]
        [StringLength(MaxDescriptionLength,
            ErrorMessage = "The {0} should be between {2} and {1} letters",
            MinimumLength = MinDescriptionLength)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Show From")]
        public DateTime ShowFrom { get; set; } = DateTime.UtcNow;
        [Required]
        [Display(Name = "Show To")]
        public DateTime ShowTo { get; set; } = DateTime.UtcNow;
        [Required]
        [Display(Name = "Informative Messages Heading")]
        public string InformativeMessagesHeadingId { get; set; }
        public IEnumerable<SelectListItem> InformativeMessagesHeadings { get; set; }
    }
}
