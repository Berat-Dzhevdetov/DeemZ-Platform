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
    using DeemZ.Services.FileService;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Models.ViewModels.Resources;

    using static Constants;

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

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Add(string lectureId)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return BadRequest();

            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            var formModel = new AddResourceFormModel();

            formModel.ResourceTypes = resourceService.GetResourceTypes<ResourceTypeFormModel>();

            return View(formModel);
        }

        [HttpPost]
        [Authorize(Roles = AdminRoleName)]
        public IActionResult Add(string lectureId, AddResourceFormModel resource,IFormFile file)
        {
            if (guard.AgainstNull(lectureId, nameof(lectureId))) return BadRequest();

            if (!resourceService.IsValidResourceType(resource.ResourceTypeId)) ModelState.AddModelError("ResourceTypes", "Invalid resource type");

            if (!ModelState.IsValid)
            {
                resource.ResourceTypes = resourceService.GetResourceTypes<ResourceTypeFormModel>();
                return View(resource);
            }

            if (!lectureService.GetLectureById(lectureId)) return NotFound();

            //trying to upload the file to the cloud
            var path = fileService.PreparingFileForUploadAndUploadIt(file, resource.Path);

            if (path == null)
            {
                ModelState.AddModelError("Path", "An error occurred while uploading file");
                return View(resource);
            }

            resource.Path = path;

            resourceService.AddResourceToLecture(lectureId, resource);

            return RedirectToAction(nameof(AdministrationController.Resources), "Administration", new { lectureId });
        }

        [Authorize]
        public IActionResult ViewResource(string resourceId)
        {
            if (guard.AgainstNull(resourceId, nameof(resourceId))) return BadRequest();

            if (!resourceService.GetResourceById(resourceId)) return NotFound();

            var userId = this.User.GetId();

            if (!resourceService.DoesUserHavePermissionToThisResource(resourceId, userId)) return Unauthorized();

            var resource = resourceService.GetResourceById<DetailsResourseViewModel>(resourceId);

            return View(resource);
        }

        [Authorize]
        public IActionResult Download(string resourceId)
        {
            if (guard.AgainstNull(resourceId, nameof(resourceId))) return BadRequest();

            if (!resourceService.GetResourceById(resourceId)) return NotFound();

            var userId = this.User.GetId();

            if (!resourceService.DoesUserHavePermissionToThisResource(resourceId, userId)) return Unauthorized();

            (byte[] bytes, string contentType, string downloadName) = fileService.GetFileBytesByResourceId(resourceId);

            this.Response.Headers.Add("Content-Disposition", "attachment");

            return File(bytes, contentType, downloadName);
        }
    }
}