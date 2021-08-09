using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DeemZ.Data.Models;
using DeemZ.Services.FileService;
using DeemZ.Services.UserServices;

namespace DeemZ.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileService fileService;
        private readonly IUserService userServices;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IFileService fileService, IUserService userServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.fileService = fileService;
            this.userServices = userServices;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [Display(Name = "Profile image")]
        public IFormFile Img { get; set; }

        public string ImgUrl { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var profileImg = user.ImgUrl;

            Username = userName;
            ImgUrl = profileImg;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if(Img != null)
            {
                var isImg = fileService.CheckIfFileIsImage(Img);

                if (!isImg)
                {
                    ModelState.AddModelError(nameof(Img), "Invalid or not supported picture");
                    goto afterImg;
                }

                var isImgTooBig = fileService.CheckIfFileIsUnderMB(Img);

                if (isImgTooBig)
                {
                    ModelState.AddModelError(nameof(Img), "The image is too big");
                    goto afterImg;
                }

                userServices.DeleteUserProfileImg(user.Id);

                (string url,string publicId) = fileService.PreparingFileForUploadAndUploadIt(Img);

                userServices.SetProfileImg(user.Id, url, publicId);
            }

        afterImg:;

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
