namespace DeemZ.Web.Controllers
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

    using static DeemZ.Global.WebConstants.Constant;

    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;

        public UserController(IUserService userService, IFileService fileService)
        {
            this.userService = userService;
            this.fileService = fileService;
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        public async Task<IActionResult> Edit(string userId)
        {
            if (!userService.GetUserById(userId)) return NotFound();

            var user = userService.GetUserById<EditUserFormModel>(userId);

            user.IsAdmin = await userService.IsInRoleAsync(userId, Role.AdminRoleName);

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        [ClientRequired]
        public async Task<IActionResult> Edit(string userId, EditUserFormModel user)
        {
            if (user == null) return NotFound();

            var isEmailFree = userService.IsEmailFree(userId, user.Email);
            var isUsernameFree = userService.IsUsernameFree(userId, user.UserName);

            if (isEmailFree)
                ModelState.AddModelError(nameof(EditUserFormModel.Email), "The given email is already taken");

            if (isUsernameFree)
                ModelState.AddModelError(nameof(EditUserFormModel.UserName), "The given username is already taken");

            if (!ModelState.IsValid) return View(user);

            await userService.EditUser(userId, user);

            if (user.IsAdmin && !await userService.IsInRoleAsync(userId, Role.AdminRoleName))
                await userService.AddUserToRole(userId, Role.AdminRoleName);
            else if (!user.IsAdmin && await userService.IsInRoleAsync(userId, Role.AdminRoleName))
                await userService.RemoveUserFromRole(userId, Role.AdminRoleName);

            return RedirectToAction(nameof(AdministrationController.Users), typeof(AdministrationController).GetControllerName(), new { area = AreaName.AdminArea });
        }

        [Authorize]
        public IActionResult DeleteProfilePicture()
        {
            var userId = User.GetId();

            userService.DeleteUserProfileImg(userId);

            userService.SetProfileImg(userId, Data.DataConstants.User.DefaultProfilePictureUrl, null);

            return LocalRedirect("/Identity/Account/Manage");
        }
    }
}