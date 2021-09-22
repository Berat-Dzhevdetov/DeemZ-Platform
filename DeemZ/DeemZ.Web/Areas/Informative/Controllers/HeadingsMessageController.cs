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
    public class HeadingsMessageController : Controller
    {
        private readonly IInformativeMessageService informativeMessageService;

        public HeadingsMessageController(IInformativeMessageService informativeMessageService)
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

        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string informativeMessagesHeadingId)
        {
            var viewModel = informativeMessageService.GetInformativeMessagesHeading<InformativeMessageHeadingFormModel>(informativeMessagesHeadingId);

            return View(viewModel);
        }

        [HttpPost]
        [ClientRequired]
        [IfExists]
        public IActionResult Edit(string informativeMessagesHeadingId, InformativeMessageHeadingFormModel message)
        {
            if (!ModelState.IsValid) return View(message);

            informativeMessageService.EditInformativeMessagesHeading(informativeMessagesHeadingId, message.Title);

            return RedirectToAction(nameof(AdministrationController.InformativeMessagesHeadings), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string informativeMessagesHeadingId)
        {
            informativeMessageService.DeleteInformativeMessagesHeading(informativeMessagesHeadingId);

            return RedirectToAction(nameof(AdministrationController.InformativeMessagesHeadings), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }
    }
}
