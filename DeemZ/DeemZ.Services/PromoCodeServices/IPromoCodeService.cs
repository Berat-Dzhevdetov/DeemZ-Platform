namespace DeemZ.Services.PromoCodeServices
{
    using DeemZ.Data.Models;
    using System.Collections.Generic;

    public interface IPromoCodeService
    {
        PromoCode GetPromoCode(string promoCode);
        void MarkPromoCodeAsUsed(string pcid);
        bool ValidatePromoCode(string uid, string promoCode);
        int GetPromoCodesCount();
        IEnumerable<T> GetPromoCodes<T>(string promoCode);
        IEnumerable<T> GetPromoCodes<T>(int page = 1 , int quantity = 20);
    }
}
