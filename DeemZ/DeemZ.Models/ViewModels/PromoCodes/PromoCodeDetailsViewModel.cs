namespace DeemZ.Models.ViewModels.PromoCodes
{
    using System;
    public class PromoCodeDetailsViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public decimal DiscountPrice { get; set; }
        public string ApplicationUserUserName { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}