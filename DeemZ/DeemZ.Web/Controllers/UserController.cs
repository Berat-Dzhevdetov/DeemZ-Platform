namespace DeemZ.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using DeemZ.Services;
    using DeemZ.Services.UserServices;
    using DeemZ.Models.FormModels.User;

    using static Constants;

    [Authorize(Roles = AdminRoleName)]
    public class UserController : Controller
    {
        private readonly Guard guard;
        private readonly IUserService userService;

        public UserController(Guard guard, IUserService userService)
        {
            this.guard = guard;
            this.userService = userService;
        }

        public async Task<IActionResult> Edit(string userId)
        {
            if (guard.AgainstNull(userId, nameof(userId))) return BadRequest();

            var user = await userService.GetUserById<EditUserFormModel>(userId);

            if (user == null) return NotFound();

            user.IsAdmin = await userService.IsInRole(userId, AdminRoleName);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, EditUserFormModel user)
        {
            if (guard.AgainstNull(userId, nameof(userId))) return BadRequest();

            if (user == null) return NotFound();

            var isEmailFree = userService.IsEmailFree(userId, user.Email);
            var isUsernameFree = userService.IsUsernameFree(userId, user.UserName);

            if (isEmailFree)
                ModelState.AddModelError("Email", "The given email is already taken");

            if (isUsernameFree)
                ModelState.AddModelError("Email", "The given username is already taken");

            if (!ModelState.IsValid) return View(user);

            await userService.EditUser(userId, user);

            if (user.IsAdmin && !await userService.IsInRole(userId, AdminRoleName))
                await userService.AddUserToRole(userId, AdminRoleName);
            else if (!user.IsAdmin && await userService.IsInRole(userId, AdminRoleName))
                await userService.RemoveUserFromRole(userId, AdminRoleName);

            return RedirectToAction(nameof(AdministrationController.Users), "Administration");
        }
    }
}