﻿namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using DeemZ.Services.UserServices;
    using DeemZ.Models.FormModels.User;
    using DeemZ.Web.Infrastructure;
    using DeemZ.Web.Areas.Administration.Controllers;
    using DeemZ.Services.FileService;
    using DeemZ.Web.Filters;
    using DeemZ.Services.CertificateServices;
    using DeemZ.Models.ViewModels.Certificates;
    using DeemZ.Services;

    using static DeemZ.Global.WebConstants.Constant;
    using DeemZ.Models.ViewModels.User;

    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;
        private readonly ICertificateService certificateService;
        private readonly Guard guard;

        public UserController(IUserService userService, IFileService fileService, ICertificateService certificateService, Guard guard)
        {
            this.userService = userService;
            this.fileService = fileService;
            this.certificateService = certificateService;
            this.guard = guard;
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        public async Task<IActionResult> Edit(string userId)
        {
            if (!userService.GetUserById(userId)) return NotFound();

            var user = await userService.GetUserById<EditUserFormModel>(userId);

            user.IsAdmin = await userService.IsInRoleAsync(userId, Role.AdminRoleName);

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        public async Task<IActionResult> Edit(string userId, EditUserFormModel user)
        {
            if (user == null) return HandleErrorRedirect(Models.HttpStatusCodes.NotFound);

            var isEmailFree = userService.IsEmailFree(userId, user.Email);
            var isUsernameFree = userService.IsUsernameFree(userId, user.UserName);

            if (!isEmailFree)
                ModelState.AddModelError(nameof(EditUserFormModel.Email), "The given email is already taken");

            if (!isUsernameFree)
                ModelState.AddModelError(nameof(EditUserFormModel.UserName), "The given username is already taken");

            if (!ModelState.IsValid) return View(user);

            await userService.EditUser(userId, user);

            if (user.IsAdmin && !await userService.IsInRoleAsync(userId, Role.AdminRoleName))
                await userService.AddUserToRole(userId, Role.AdminRoleName);
            else if (!user.IsAdmin && await userService.IsInRoleAsync(userId, Role.AdminRoleName))
                await userService.RemoveUserFromRole(userId, Role.AdminRoleName);

            return RedirectToAction(nameof(AdministrationController.Users), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        public IActionResult ViewCertificate(int id)
        {
            if (guard.AgainstNull(id, nameof(id)))
                return HandleErrorRedirect(Models.HttpStatusCodes.BadRequest);

            var certificate = certificateService.GetCertificateByExternalNumber<CertificateBasicViewModel>(id);

            if (certificate == null)
                return HandleErrorRedirect(Models.HttpStatusCodes.NotFound);

            return View(certificate);
        }

        [Authorize]
        public IActionResult MyCertificates()
        {
            var userId = User.GetId();

            var certificates = certificateService.GetUserCertificates<CertificateDetailsViewModel>(userId);

            return View(certificates);
        }

        [Authorize]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var userId = User.GetId();

            await userService.DeleteUserProfileImg(userId);

            await userService.SetProfileImg(userId, Data.DataConstants.User.DefaultProfilePictureUrl, null);

            return LocalRedirect("/Identity/Account/Manage");
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        [IfExists]
        public async Task<IActionResult> Profile(string userId)
        {
            if (await userService.UserExists(userId))
                HandleErrorRedirect(Models.HttpStatusCodes.NotFound);

            var userInformation = await userService.GetUserInformation(userId);
            
            return View(userInformation);
        }
    }
}