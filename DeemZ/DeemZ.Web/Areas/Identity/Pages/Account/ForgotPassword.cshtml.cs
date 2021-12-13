namespace DeemZ.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using DeemZ.Services.EmailSender;

    using static DeemZ.Global.WebConstants.Constant;
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var content = $@"
<div>
Hello {user.UserName},<br>
Someone has just requested that the password needs to be reset in {EmailSender.Name}. We, from the team of {EmailSender.Name} cannot be sure whether you have requested a new password or not, but we are obliged to warn you that if you receive this email without requesting a new password then do not press the button below. But if you forgot the password, which happens often to everyone, and you just want a new one then press the button.<br>
    
    <div style='margin: 10px 0; text-align:center;'>
    <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='border-radius:15px;text-decoration:none;color:white;padding: 10px 15px;text-align: center;background-color:#E9806E'>Reset password</a>
    </div>

   In other case just ignore this message.
</div>";

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    content);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
