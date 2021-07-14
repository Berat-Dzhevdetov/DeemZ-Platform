namespace DeemZ.Web.Controllers
{
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.Forum;
    using DeemZ.Services;
    using DeemZ.Services.ForumService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        public IActionResult Index(int page, int quantity, string Search)
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
                models = forumService.GetTopicsByTitleName<ForumTopicsViewModel>(Search, page, quantity);
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

            if (topic == null) return NotFound();

            var viewModelTest = new ViewAndFormModelForTopics();

            viewModelTest.ViewModel = topic;

            return View(viewModelTest);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostCommentAsync(string topicId, ViewAndFormModelForTopics model)
        {
            if (guard.AgainstNull(topicId, nameof(topicId))) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ViewModel = forumService.GetTopicById<TopicViewModel>(topicId);
                return View(nameof(Topic), model);
            }

            var user = await this.userManager.GetUserAsync(HttpContext.User);

            forumService.CreateComment(model.FormModel, topicId, user.Id);

            model.ViewModel = forumService.GetTopicById<TopicViewModel>(topicId);

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
