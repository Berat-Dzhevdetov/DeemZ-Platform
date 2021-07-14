namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using DeemZ.Services;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Services.ForumService;
    using DeemZ.Models.ViewModels.Forum;

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

        public IActionResult Index(int page, int quantity,string Search)
        {
            if (guard.AgainstNull(page, nameof(page))) page = 1;
            if (guard.AgainstNull(quantity, nameof(quantity))) quantity = 10;

            if (quantity <= 0) quantity = 10;

            var allPages = (int)Math.Ceiling(forumService.Count() / (quantity * 1.0));

            if (page <= 0 || page > allPages) page = 1;

            IEnumerable<ForumTopicsViewModel> models = null;

            if (guard.AgainstNull(Search, nameof(Search)))
            {
                models = forumService.GetAllTopics<ForumTopicsViewModel>(page, quantity);
            }
            else
            {
                models = forumService.GetTopicsByTitleName<ForumTopicsViewModel>(Search,page,quantity);
            }

            var viewModel = new AllForumTopicsViewModel()
            {
                Topics = models
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

            return RedirectToAction(nameof(Topic), new { topicId = topicId });
        }

        public IActionResult Topic(string topicId)
        {
            if (guard.AgainstNull(topicId, nameof(topicId))) return NotFound();

            var topic = forumService.GetTopicById<TopicViewModel>(topicId);

            return View(topic);
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostComment(string topicId, string text)
        {
            var topic = forumService.GetTopicById<TopicViewModel>(topicId);

            if (!ModelState.IsValid) return View(nameof(Topic), new { topic, text });

            return RedirectToAction(nameof(Topic), new { topicId = topicId });
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
