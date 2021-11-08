namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using System;
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
    using DeemZ.Web.Filters;
    using DeemZ.Models.Shared;

    using static DeemZ.Global.WebConstants.Constant;

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

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Add(string lectureId)
        {
            var formModel = new AddResourceFormModel
            {
                ResourceTypes = resourceService.GetResourceTypes<ResourceTypeFormModel>()
            };

            return View(formModel);
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        [DisableRequestSizeLimit,
                 RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue,
                 ValueLengthLimit = int.MaxValue)]
        [ClientRequired("lectureId")]
        [IfExists]
        public IActionResult Add(string lectureId, AddResourceFormModel resource, IFormFile file)
        {
            if (!resourceService.IsValidResourceType(resource.ResourceTypeId)) ModelState.AddModelError(nameof(resource.ResourceTypes), "Invalid resource type");

            if (!ModelState.IsValid)
            {
                resource.ResourceTypes = resourceService.GetResourceTypes<ResourceTypeFormModel>();
                return View(resource);
            }

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
                ModelState.AddModelError(nameof(resource.Path), "An error occurred while uploading file");
                return View(resource);
            }

            resourceService.AddResourceToLecture(lectureId, publicId, resource);

            return RedirectToAction(nameof(AdministrationController.Resources), typeof(AdministrationController).GetControllerName(), new { lectureId, area = AreaName.AdminArea });
        }

        [Authorize]
        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> ViewResource(string resourceId)
        {
            var userId = User.GetId();

            var isAdmin = await userService.IsInRoleAsync(userId, Role.AdminRoleName);

            if (!resourceService.DoesUserHavePermissionToThisResource(resourceId, userId) && !isAdmin) return Unauthorized();

            var resource = resourceService.GetResourceById<DetailsResourseViewModel>(resourceId);

            if (resource.IsRemote) return NotFound();

            ViewBag.IsVideo = false;

            if (resource.ResourceTypeName == "Video") ViewBag.IsVideo = true;

            Response.Headers.Add("Content-Type", "text/html");

            return View(resource);
        }

        [Authorize]
        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> Download(string resourceId)
        {
            var userId = User.GetId();

            var isAdmin = await userService.IsInRoleAsync(userId, Role.AdminRoleName);

            if (!resourceService.DoesUserHavePermissionToThisResource(resourceId, userId) && !isAdmin) return Unauthorized();

            (byte[] bytes, string contentType, string downloadName) = fileService.GetFileBytesByResourceId(resourceId);

            Response.Headers.Add("Content-Disposition", "attachment");

            return File(bytes, contentType, downloadName);
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public IActionResult Delete(string resourceId)
        {
            var lectureId = resourceService.Delete(resourceId);

            return RedirectToAction(nameof(AdministrationController.Resources), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea, lectureId });
        }

        public IActionResult MyResources(int page = 1, int quantity = 20)
        {
            var viewModel = new MyResourcesViewModel();

            var userId = User.GetId();

            var allPages = (int)Math.Ceiling(resourceService.GetUserResourcesCount(userId) / (quantity * 1.0));

            viewModel.Resources = resourceService.GetUserResources<DetailsResourseViewModel>(userId, page, quantity);

            viewModel = AdjustPages(viewModel, page, allPages);

            return View(viewModel);
        }

        private static T AdjustPages<T>(T viewModel, int page, int allPages)
            where T : PagingBaseModel
        {
            viewModel.CurrentPage = page;
            viewModel.NextPage = page >= allPages ? null : page + 1;
            viewModel.PreviousPage = page <= 1 ? null : page - 1;
            viewModel.MaxPages = allPages;

            return viewModel;
        }
    }
}