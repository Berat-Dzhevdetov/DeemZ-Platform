namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.Forum;
    using DeemZ.Services;
    using DeemZ.Services.ForumServices;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Filters;
    using System.Threading.Tasks;

    public class ForumController : Controller
    {
        private readonly Guard guard;
        private readonly IForumService forumService;

        public ForumController(IForumService forumService, Guard guard)
        {
            this.forumService = forumService;
            this.guard = guard;
        }

        public IActionResult Index(string Search,int page = 1, int quantity = 10)
        {
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

        [Authorize]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        [ClientRequired]
        public IActionResult Create(CreateForumTopicFormModel topic)
        {
            if (!ModelState.IsValid) return View(topic);

            var userId = User.GetId();

            var topicId = forumService.CreateTopic(topic, userId);

            return RedirectToAction(nameof(Topic), new { topicId });
        }

        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> Topic(string forumId)
        {
            var topic = await forumService.GetTopicById<TopicViewModel>(forumId);

            var viewModelTest = new ViewAndFormModelForTopics
            {
                ViewModel = topic
            };

            return View(viewModelTest);
        }

        [HttpPost]
        [Authorize]
        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> PostCommentAsync(string forumId, ViewAndFormModelForTopics model)
        {
            if (!ModelState.IsValid)
            {
                model.ViewModel = await forumService.GetTopicById<TopicViewModel>(forumId);
                return View(nameof(Topic), model);
            }

            var userId = User.GetId();

            await forumService.CreateComment(model.FormModel, forumId, userId);

            model.ViewModel = await forumService.GetTopicById<TopicViewModel>(forumId);

            return RedirectToAction(nameof(Topic), new { forumId });
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
