namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using DeemZ.Models;
    using DeemZ.Models.DTOs;
    using DeemZ.Services.UserServices;
    using DeemZ.Services.InformativeMessageServices;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models.ViewModels.InformativeMessages;

    using static Global.WebConstants.Constant;
    using static Global.WebConstants.UserErrorMessages;

    public class HomeController : BaseController
    {
        private readonly IUserService userService;
        private readonly IMemoryCache memoryCache;
        private readonly IInformativeMessageService informativeMessageService;

        public HomeController(IUserService userService, IMemoryCache memoryCache, IInformativeMessageService informativeMessageService)
        {
            this.userService = userService;
            this.memoryCache = memoryCache;
            this.informativeMessageService = informativeMessageService;
        }

        public async Task<IActionResult> Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var userId = User.GetId();

                var isAdmin = await userService.IsInRoleAsync(userId, Role.AdminRoleName);

                var informativeMessages = memoryCache.Get<List<InformativeMessagesHeadingViewModel>>(InformativeMessagesCacheKey);

                if(informativeMessages == null)
                {
                    informativeMessages = (List<InformativeMessagesHeadingViewModel>)informativeMessageService.GetInformativeMessages<InformativeMessagesHeadingViewModel>();

                    var memoryChacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)).SetPriority(CacheItemPriority.High);

                    memoryCache.Set(InformativeMessagesCacheKey, informativeMessages, memoryChacheEntryOptions);
                }

                var viewModel = userService.GetIndexInformaiton(userId, !isAdmin);

                viewModel.InformativeMessagesHeadings = informativeMessages;

                return View("LoggedIndex", viewModel);
            }
            return View();
        }

        public IActionResult UserErrorPage()
        {
            string errorMessageJson = null;
            try
            {
                errorMessageJson = TempData[ErrorMessageKey].ToString();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorMessage = JsonConvert.DeserializeObject<HandleErrorModel>(errorMessageJson);

            return View(errorMessage);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}