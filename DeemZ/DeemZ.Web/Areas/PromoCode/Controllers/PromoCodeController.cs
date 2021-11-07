namespace DeemZ.Web.Areas.Informative.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services.PromoCodeServices;
    using DeemZ.Models.FormModels.PromoCode;
    using DeemZ.Services.UserServices;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Filters;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.PromoCodeArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class PromoCodeController : Controller
    {
        private readonly IPromoCodeService promoCodeService;
        private readonly IUserService userService;

        public PromoCodeController(IPromoCodeService promoCodeService, IUserService userService)
        {
            this.promoCodeService = promoCodeService;
            this.userService = userService;
        }

        public IActionResult Add(string userName = null, string returnUrl = null)
        {
            var promoCodeModel = new AddPromoCodeFormModel()
            {
                UserName = userName
            };

            return View(promoCodeModel);
        }

        [HttpPost]
        public IActionResult Add(AddPromoCodeFormModel promoCode, string returnUrl = null)
        {
            if (!userService.GetUserByUserName(promoCode.UserName))
                ModelState.AddModelError(nameof(promoCode.UserName), "Given user name is invalid.");

            if(promoCodeService.IfExists(promoCode.Text))
                ModelState.AddModelError(nameof(promoCode.Text), "Sorry but the given text already exists. Try again with the button.");

            if (!ModelState.IsValid)
                return View(promoCode);

            promoCodeService.AddPromoCode(promoCode);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(AdministrationController.PromoCodes), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string promoCodeId)
        {
            var promoCode = promoCodeService.GetPromoCodeById<EditPromoCodeFormModel>(promoCodeId);
            promoCode.ExpireOn = promoCode.ExpireOn.ToLocalTime();
            return View(promoCode);
        }

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public IActionResult Edit(string promoCodeId, EditPromoCodeFormModel promoCode)
        {
            if (!userService.GetUserByUserName(promoCode.ApplicationUserUserName))
                ModelState.AddModelError(nameof(promoCode.ApplicationUserUserName), "Given user name is invalid.");

            var promoCodeDb = promoCodeService.GetPromoCode(promoCode.Text);

            if (promoCodeService.IfExists(promoCode.Text) && promoCode.Text != promoCode.Text)
                ModelState.AddModelError(nameof(promoCode.Text), "Sorry but the given text already exists. Try again with the button.");

            if (!ModelState.IsValid)
                return View(promoCode);

            promoCodeService.EditPromoCode(promoCodeId, promoCode);

            return RedirectToAction(nameof(AdministrationController.PromoCodes), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string promoCodeId)
        {
            promoCodeService.Delete(promoCodeId);

            return RedirectToAction(nameof(AdministrationController.PromoCodes), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        public IActionResult DeleteAllExpiredCodes()
        {
            promoCodeService.DeleteAllExipiredCodes();

            return RedirectToAction(nameof(AdministrationController.PromoCodes), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GeneratePromoCodeText() =>
            Ok(await promoCodeService.GeneratePromoCodeText());
    }
}
