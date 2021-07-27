namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using DeemZ.Services;
    using DeemZ.Services.ResourceService;
    using DeemZ.Services.LectureServices;
    using DeemZ.Models.FormModels.Resource;
    using DeemZ.Services.CloudinaryServices;

    using static Constants;

    [Authorize(Roles = AdminRoleName)]
    public class ResourceController : Controller
    {
        private readonly Guard guard;
        private readonly ILectureService lectureService;
        private readonly IResourceService resourceService;
        private readonly IFileServices fileService;

        public ResourceController(Guard guard, ILectureService lectureService, IResourceService resourceService, IFileServices cloudinaryService)
        {
            this.lectureService = lectureService;
            this.guard = guard;
            this.resourceService = resourceService;
            this.fileService = cloudinaryService;
        }

        public IActionResult Add(string lectureId)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return BadRequest();

            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            var formModel = new AddResourceFormModel();

            formModel.ResourceTypes = resourceService.GetResourceTypes<ResourceTypeFormModel>();

            return View(formModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string lectureId, AddResourceFormModel resource,IFormFile file)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return BadRequest();

            if (!resourceService.IsValidResourceType(resource.ResourceTypeId)) ModelState.AddModelError("ResourceTypes", "Invalid resource type");

            if (!ModelState.IsValid)
            {
                resource.ResourceTypes = resourceService.GetResourceTypes<ResourceTypeFormModel>();
                return View(resource);
            }

            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            //trying to upload the file to the file system
            var path = await fileService.UploadFile(file, resource.Path);

            if (path == null)
            {
                ModelState.AddModelError("Path", "An error occurred while uploading file");
                return View(resource);
            }
            else if (path != "url")
            {
                resource.Path = path;
            }

            resourceService.AddResourceToLecture(lectureId, resource);

            return RedirectToAction(nameof(AdministrationController.Resources), "Administration", new { lectureId });
        }
    }
}