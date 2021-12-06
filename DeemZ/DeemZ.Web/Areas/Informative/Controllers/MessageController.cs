namespace DeemZ.Web.Areas.Informative.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Web.Filters;
    using DeemZ.Models.FormModels.InformativeMessages;
    using DeemZ.Services.InformativeMessageServices;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Infrastructure;

    using static DeemZ.Global.WebConstants.Constant;

    [Area(AreaName.InformativeArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class MessageController : Controller
    {
        private readonly IInformativeMessageService informativeMessageService;

        public MessageController(IInformativeMessageService informativeMessageService)
            => this.informativeMessageService = informativeMessageService;

        [ClientRequired]
        [IfExists]
        public IActionResult Add(string informativeMessagesHeadingId) => View();

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public IActionResult Add(string informativeMessagesHeadingId, InformativeMessageFormModel message)
        {
            informativeMessageService.CreateInformativeMessage(informativeMessagesHeadingId, message);

            return RedirectToAction(nameof(AdministrationController.InformativeMessages), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea, informativeMessagesHeadingId });
        }

        public IActionResult Edit(string informativeMessageId)
        {
            var viewModel = informativeMessageService.GetInformativeMessage<InformativeMessageEditFormModel>(informativeMessageId);

            return View(viewModel);
        }

        [ClientRequired]
        [IfExists]
        [HttpPost]
        public IActionResult Edit(string informativeMessageId, InformativeMessageEditFormModel message)
        {
            if (!ModelState.IsValid)
                return View(message);

            var informativeMessagesHeadingId = informativeMessageService.EditInformativeMessage(informativeMessageId, message);

            return RedirectToAction(nameof(AdministrationController.InformativeMessages), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea, informativeMessagesHeadingId });
        }

        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string informativeMessageId)
        {
            var informativeMessagesHeadingId = informativeMessageService.DeleteInformativeMessage(informativeMessageId);

            return RedirectToAction(nameof(AdministrationController.InformativeMessages), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea, informativeMessagesHeadingId });
        }
    }
}
