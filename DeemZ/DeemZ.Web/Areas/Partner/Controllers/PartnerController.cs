namespace DeemZ.Web.Areas.Partner.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using DeemZ.Web.Filters;
    using DeemZ.Services.PartnerServices;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Controllers;
    using DeemZ.Models.FormModels.Partner;
    using DeemZ.Data.Models;
    using DeemZ.Services.CachingService;
    using System.Collections.Generic;
    using DeemZ.Data;
    using DeemZ.Models.ViewModels.Partners;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.PartnerArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class PartnerController : BaseController
    {
        private readonly IPartnerService partnerService;
        private readonly IMemoryCachingService memoryCache;

        public PartnerController(IPartnerService partnerService, IMemoryCachingService memoryCache)
        {
            this.partnerService = partnerService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Add()
        {
            var formModel = new AddPartnerFormModel()
            {
                Tiers = await partnerService.GetTiers()
            };
            return View(formModel);
        }

        [HttpPost]
        [ClientRequired]
        public async Task<IActionResult> Add(AddPartnerFormModel formModel)
        {
            if (!await partnerService.ValidateTier(formModel.Tier))
            {
                ModelState.AddModelError(nameof(formModel.Tier), "Invalid tier rank");
            }

            if (!ModelState.IsValid)
            {
                formModel.Tiers = await partnerService.GetTiers();
                return View(formModel);
            }

            await partnerService.Create(formModel);

            return RedirectToAction(nameof(AdministrationController.Partners), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        public async Task<IActionResult> Edit(string partnerId)
        {
            var formModel = await partnerService.GetPartnerById<EditPartnerFormModel>(partnerId);
            formModel.Tiers = await partnerService.GetTiers();
            return View(formModel);
        }

        [HttpPost]
        [ClientRequired]
        public async Task<IActionResult> Edit(string partnerId, EditPartnerFormModel formModel)
        {
            if (!await partnerService.ValidateTier((int)formModel.Tier))
            {
                ModelState.AddModelError(nameof(formModel.Tier), "Invalid tier rank");
            }

            if (!ModelState.IsValid)
            {
                formModel.Tiers = await partnerService.GetTiers();
                return View(formModel);
            }

            await partnerService.Edit(partnerId, formModel);

            return RedirectToAction(nameof(AdministrationController.Partners), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [AllowAnonymous]
        public IActionResult All()
        {
            if(!memoryCache.ItemExists(CachingKey.PartnersCacheKey, out List<IGrouping<PartnerTiers, PartnerBasicDetailsViewModel>> partners))
            {
                partners = partnerService.GetAllPartners();

                memoryCache.CreateItem(CachingKey.PartnersCacheKey, partners, TimeSpan.FromDays(1));
            }

            return View(partners);
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string partnerId)
        {
            partnerService.Delete(partnerId);

            return RedirectToAction(nameof(AdministrationController.Partners), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }
    }
}