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

    public class ForumController : Controller
    {
        private readonly IForumService forumService;
        private readonly Guard guard;

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

        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateForumTopicFormModel topic)
        {
            if (!ModelState.IsValid) return View(topic);

            var userId = User.GetId();

            var topicId = forumService.CreateTopic(topic, userId);

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
        public IActionResult PostComment(string topicId, ViewAndFormModelForTopics model)
        {
            if (guard.AgainstNull(topicId, nameof(topicId))) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ViewModel = forumService.GetTopicById<TopicViewModel>(topicId);
                return View(nameof(Topic), model);
            }

            var userId = User.GetId();

            forumService.CreateComment(model.FormModel, topicId, userId);

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
