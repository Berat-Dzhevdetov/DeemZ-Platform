namespace DeemZ.Web.Controllers
{
    using DeemZ.Services;
    using DeemZ.Services.CourseServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    //For Admin only
    [Authorize]
    public class ResourceController : Controller
    {
        private readonly Guard guard;
        private readonly ICourseService courseService;

        public ResourceController(ICourseService courseService, Guard guard)
        {
            this.courseService = courseService;
            this.guard = guard;
        }

        
    }
}
