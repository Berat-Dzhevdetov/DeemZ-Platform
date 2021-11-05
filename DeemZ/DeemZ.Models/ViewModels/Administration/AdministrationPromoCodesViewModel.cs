namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.PromoCodes;

    public class AdministrationPromoCodesViewModel : PagingBaseModel
    {
        public IEnumerable<PromoCodeDetailsViewModel> PromoCodes { get; set; } = new HashSet<PromoCodeDetailsViewModel>();
        public string PromoCode { get; set; }
    }
}
