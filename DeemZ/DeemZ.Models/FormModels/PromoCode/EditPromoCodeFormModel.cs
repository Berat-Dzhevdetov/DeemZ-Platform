namespace DeemZ.Models.FormModels.PromoCode
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.PromoCode;

    public class EditPromoCodeFormModel
    {
        [Required]
        [StringLength(TextLength, MinimumLength = TextLength)]
        public string Text { get; set; }
        [Required]
        [Range(MinDiscountPrice, MaxDiscountPrice)]
        [Display(Name = "Discount Price")]
        public decimal DiscountPrice { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string ApplicationUserUserName { get; set; }
        [Required]
        [Display(Name = "Expire On")]
        public DateTime ExpireOn { get; set; } = DateTime.Now;
    }
}
