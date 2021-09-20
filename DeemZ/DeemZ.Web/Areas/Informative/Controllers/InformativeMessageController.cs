namespace DeemZ.Web.Areas.Informative.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Web.Filters;
    using DeemZ.Models.FormModels.InformativeMessages;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Services.InformativeMessageServices;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.InformativeArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class InformativeMessageController : Controller
    {
        private readonly IInformativeMessageService informativeMessageService;

        public InformativeMessageController(IInformativeMessageService informativeMessageService)
        {
            this.informativeMessageService = informativeMessageService;
        }

        public IActionResult Add() => View();

        [HttpPost]
        [ClientRequired]
        public IActionResult Add(InformativeMessageHeadingFormModel message)
        {
            if (!ModelState.IsValid) return View(message);

            informativeMessageService.CreateInformativeMessagesHeading(message.Title);

            return RedirectToAction(nameof(AdministrationController.InformativeMessagesHeadings), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }
    }
}
