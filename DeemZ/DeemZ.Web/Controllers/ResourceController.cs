namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services;
    using DeemZ.Services.ResourceService;
    using DeemZ.Services.LectureServices;

    using static Constants;

    [Authorize(Roles = AdminRoleName)]
    public class ResourceController : Controller
    {
        private readonly Guard guard;
        private readonly ILectureService lectureService;
        private readonly IResourceService resourceService;

        public ResourceController(Guard guard, ILectureService lectureService, IResourceService resourceService)
        {
            this.lectureService = lectureService;
            this.guard = guard;
            this.resourceService = resourceService;
        }

        public IActionResult Add(string lectureId)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return BadRequest();

            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            return View();
        }
    }
}