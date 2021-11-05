namespace DeemZ.Web.Areas.Informative.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.PromoCodeServices;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.PromoCodeArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class PromoCodeController : Controller
    {
        private readonly IPromoCodeService promoCodeService;

        public PromoCodeController(IPromoCodeService promoCodeService)
        {
            this.promoCodeService = promoCodeService;
        }
    }
}
