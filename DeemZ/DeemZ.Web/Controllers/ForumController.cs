namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;

    public class ForumController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IForumService forumService
        public ForumController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateForumTopicFormModel topic)
        {
            if (!ModelState.IsValid) return View(topic);



            return View();
        }
    }
}
