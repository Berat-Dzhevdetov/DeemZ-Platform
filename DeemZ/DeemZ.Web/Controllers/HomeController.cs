namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DeemZ.Models;
    using DeemZ.Models.DTOs;
    using DeemZ.Services.UserServices;
    using DeemZ.Services.InformativeMessageServices;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models.ViewModels.InformativeMessages;
    using DeemZ.Services.CachingService;
    using DeemZ.Models.ViewModels.Course;
    using DeemZ.Services.CourseServices;

    using static Global.WebConstants.Constant;
    using static Global.WebConstants.UserErrorMessages;

    public class HomeController : BaseController
    {
        private readonly IUserService userService;
        private readonly IMemoryCachingService cachingService;
        private readonly IInformativeMessageService informativeMessageService;
        private readonly ICourseService courseService;

        public HomeController(IUserService userService, IInformativeMessageService informativeMessageService, IMemoryCachingService cachingService, ICourseService courseService)
        {
            this.userService = userService;
            this.informativeMessageService = informativeMessageService;
            this.cachingService = cachingService;
            this.courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var userId = User.GetId();

                var isAdmin = await userService.IsInRoleAsync(userId, Role.AdminRoleName);
                
                if (!cachingService.ItemExists<IEnumerable<InformativeMessagesHeadingViewModel>>(InformativeMessagesCacheKey, out var informativeMessages))
                {
                    informativeMessages = informativeMessageService.GetInformativeMessages<InformativeMessagesHeadingViewModel>();
                    
                    cachingService.CreateItem(InformativeMessagesCacheKey, informativeMessages,TimeSpan.FromMinutes(30));
                }

                if(!cachingService.ItemExists<IEnumerable<IndexSignUpForCourseViewModel>>(UpCommingCoursesCacheKey, out var upCommingCourses))
                {
                    upCommingCourses = courseService.GetCoursesForSignUp<IndexSignUpForCourseViewModel>();

                    cachingService.CreateItem(UpCommingCoursesCacheKey, upCommingCourses, TimeSpan.FromMinutes(30));
                }

                var viewModel = userService.GetIndexInformaiton(userId, !isAdmin);

                viewModel.InformativeMessagesHeadings = informativeMessages;
                viewModel.SignUpCourses = upCommingCourses;

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