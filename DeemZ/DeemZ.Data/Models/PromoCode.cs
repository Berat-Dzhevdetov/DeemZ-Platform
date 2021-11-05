namespace DeemZ.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.PromoCode;
    public class PromoCode : BaseModel
    {
        [Required]
        [MaxLength(MaxTextLength)]
        public string Text { get; set; }
        [Required]
        [Range(20, 150)]
        public decimal DiscountPrice { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public bool IsUsed { get; set; }
        [Required]
        public DateTime ExpireOn { get; set; } = DateTime.UtcNow;
    }
}
