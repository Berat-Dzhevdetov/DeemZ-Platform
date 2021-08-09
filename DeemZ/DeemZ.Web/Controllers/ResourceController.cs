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
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Services.UserServices;

    using static DeemZ.Global.WebConstants.Constants;

    public class ResourceController : Controller
    {
        private readonly Guard guard;
        private readonly ILectureService lectureService;
        private readonly IResourceService resourceService;
        private readonly IFileService fileService;
        private readonly IUserService userService;

        public ResourceController(Guard guard, ILectureService lectureService, IResourceService resourceService, IFileService fileService, IUserService userService)
        {
            this.lectureService = lectureService;
            this.guard = guard;
            this.resourceService = resourceService;
            this.fileService = fileService;
            this.userService = userService;
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
        [DisableRequestSizeLimit,
                 RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
                 ValueLengthLimit = int.MaxValue)]
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

            string path = string.Empty;
            string publicId = string.Empty;

            if (file != null)
            {
                //trying to upload the file to the cloud
                (path, publicId) = fileService.PreparingFileForUploadAndUploadIt(file, resource.Path);
                resource.Path = path;
            }

            if (path == null && publicId == null)
            {
                ModelState.AddModelError("Path", "An error occurred while uploading file");
                return View(resource);
            }

            resourceService.AddResourceToLecture(lectureId, publicId, resource);

            return RedirectToAction(nameof(AdministrationController.Resources), typeof(AdministrationController).GetControllerName(), new { lectureId, area = AreaNames.AdminArea });
        }

        [Authorize]
        public async Task<IActionResult> ViewResource(string resourceId)
        {
            if (guard.AgainstNull(resourceId, nameof(resourceId))) return BadRequest();

            if (!resourceService.GetResourceById(resourceId)) return NotFound();

            var userId = User.GetId();

            var isAdmin = await userService.IsInRole(userId, AdminRoleName);

            if (!resourceService.DoesUserHavePermissionToThisResource(resourceId, userId) && !isAdmin) return Unauthorized();

            var resource = resourceService.GetResourceById<DetailsResourseViewModel>(resourceId);

            if (resource.IsRemote) return NotFound();

            ViewBag.IsVideo = false;

            if (resource.ResourceTypeName == "Video") ViewBag.IsVideo = true;

            Response.Headers.Add("Content-Type", "text/html");

            return View(resource);
        }

        [Authorize]
        public IActionResult Download(string resourceId)
        {
            if (guard.AgainstNull(resourceId, nameof(resourceId))) return BadRequest();

            if (!resourceService.GetResourceById(resourceId)) return NotFound();

            var userId = User.GetId();

            if (!resourceService.DoesUserHavePermissionToThisResource(resourceId, userId)) return Unauthorized();

            (byte[] bytes, string contentType, string downloadName) = fileService.GetFileBytesByResourceId(resourceId);

            Response.Headers.Add("Content-Disposition", "attachment");

            return File(bytes, contentType, downloadName);
        }

        [Authorize(Roles = AdminRoleName)]
        public IActionResult Delete(string resourceId)
        {
            if (guard.AgainstNull(resourceId, nameof(resourceId))) return BadRequest();

            if (!resourceService.GetResourceById(resourceId)) return NotFound();

            var lectureId = resourceService.Delete(resourceId);

            return RedirectToAction(nameof(AdministrationController.Resources), typeof(AdministrationController).GetControllerName(), new { area = AreaNames.AdminArea, lectureId });
        }
    }
}