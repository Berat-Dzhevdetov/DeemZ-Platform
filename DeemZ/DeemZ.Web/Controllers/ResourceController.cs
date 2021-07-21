namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.ResourceService;

    //For Admin only
    [Authorize]
    public class ResourceController : Controller
    {
        private readonly Guard guard;
        private readonly ICourseService courseService;
        private readonly IResourceService resourceService;

        public ResourceController(ICourseService courseService, Guard guard, IResourceService resourceService)
        {
            this.courseService = courseService;
            this.guard = guard;
            this.resourceService = resourceService;
        }

        public IActionResult Add(string cid)
        {
            if (guard.AgainstNull(cid, nameof(cid))) return BadRequest();

            if (courseService.GetCourseById(cid)) return NotFound();

            return View();
        }
    }
}
