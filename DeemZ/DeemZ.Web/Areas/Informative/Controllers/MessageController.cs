namespace DeemZ.Web.Areas.Informative.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Web.Filters;
    using DeemZ.Models.FormModels.InformativeMessages;
    using DeemZ.Services.InformativeMessageServices;
    using DeemZ.Models.ViewModels.InformativeMessages;
    using DeemZ.Models.DTOs.SelectListItem.InformativeMessagesHeadings;

    using static DeemZ.Global.WebConstants.Constant;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Web.Infrastructure;

    [Area(AreaName.InformativeArea)]
    [Authorize(Roles = Role.AdminRoleName)]
    public class MessageController : Controller
    {
        private readonly IInformativeMessageService informativeMessageService;

        public MessageController(IInformativeMessageService informativeMessageService)
        {
            this.informativeMessageService = informativeMessageService;
        }

        public IActionResult Add()
        {
            var headings = informativeMessageService.GetInformativeMessageHeadings<InformativeMessagesHeadingsDTO>()
                .Select(x => new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id
                });

            var viewModel = new InformativeMessageFormModel()
            {
                InformativeMessagesHeadings = headings
            };
            return View(viewModel);
        }

        [ClientRequired]
        [HttpPost]
        public IActionResult Add(InformativeMessageFormModel message)
        {
            if (!informativeMessageService.HeadingExits(message.InformativeMessagesHeadingId))
                ModelState.AddModelError(nameof(message.InformativeMessagesHeadingId), "Invalid id provided");

            if (!ModelState.IsValid)
            {
                var headings = informativeMessageService.GetInformativeMessageHeadings<InformativeMessagesHeadingsDTO>()
                .Select(x => new SelectListItem()
                {
                    Text = x.Title,
                    Value = x.Id
                });

                message.InformativeMessagesHeadings = headings;
                return View(message);
            }

            informativeMessageService.CreateInformativeMessage(message);

            return RedirectToAction(nameof(AdministrationController.InformativeMessages), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea, informativeMessagesHeadingId = message.InformativeMessagesHeadingId });
        }
    }
}
