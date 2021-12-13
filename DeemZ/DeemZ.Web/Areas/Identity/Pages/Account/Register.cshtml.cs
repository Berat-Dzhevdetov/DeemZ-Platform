namespace DeemZ.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using DeemZ.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using DeemZ.Data;
    using DeemZ.Services.EmailSender;

    using static Data.DataConstants.User;
    using static Global.WebConstants.Constant;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSenderService _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSenderService emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(DataConstants.User.MaxUsernameLength,
                          ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                          MinimumLength = DataConstants.User.MinUsernameLength)]
            [Display(Name = "Username")]
            [RegularExpression(DataConstants.User.UsernameRegex, ErrorMessage = "Username can only contains letters and numbers")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(MaxFirstNameLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = MinFirstNameLength)]
            [Display(Name = "Fisrt Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(MaxLastNameLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = MinLastNameLength)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public bool isTrue
            { get { return true; } }

            [Required]
            [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
            public bool PrivacyConfirm { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    PrivacyConfirm = Input.PrivacyConfirm,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: Request.Scheme);

                    var html = $@"
<div>
    Hello {user.UserName},<br>
    We're happy you signed up for {EmailSender.Name}. To start exploring the app, please confirm your email.
    <div style='margin: 20px 0; text-align:center;'>
        <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' style='border-radius:15px;text-decoration:none;color:white;padding: 10px 15px;text-align: center;background-color:#E9806E'>Confirm account</a>
    </div>
    
    Welcome to {EmailSender.Name}.<br>
</div>
";

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        html);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
