namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using DeemZ.Services;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Services.ForumService;
    using DeemZ.Models.ViewModels.Forum;
    using System;

    public class ForumController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IForumService forumService;
        private readonly Guard guard;

        public ForumController(UserManager<ApplicationUser> userManager, IForumService forumService, Guard guard)
        {
            this.userManager = userManager;
            this.forumService = forumService;
            this.guard = guard;
        }

        public IActionResult Index(int page, int quantity)
        {
            if (guard.AgainstNull(page, nameof(page))) page = 1;
            if (guard.AgainstNull(quantity, nameof(quantity))) quantity = 10;

            if (quantity <= 0) quantity = 10;

            var allPages = (int)Math.Ceiling(forumService.Count() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            var models = forumService.GetAllTopics<ForumTopicsViewModel>(page);

            var viewModel = new AllForumTopicsViewModel()
            {
                Topics = models,
            };

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateForumTopicFormModel topic)
        {
            if (!ModelState.IsValid) return View(topic);

            var user = await this.userManager.GetUserAsync(HttpContext.User);

            var topicId = forumService.CreateTopic(topic, user.Id);

            return RedirectToAction("Topic", new { topicId = topicId });
        }

        private AllForumTopicsViewModel AdjustPages(AllForumTopicsViewModel viewModel, int page, int allPages)
        {
            viewModel.CurrentPage = page;
            viewModel.NextPage = page >= allPages ? null : page + 1;
            viewModel.PreviousPage = page <= 1 ? null : page - 1;
            viewModel.MaxPages = allPages;

            return viewModel;
        }
    }
}
