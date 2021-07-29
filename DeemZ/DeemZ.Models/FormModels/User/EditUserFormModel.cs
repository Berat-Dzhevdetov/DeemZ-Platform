namespace DeemZ.Models.FormModels.User
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.User;

    public class EditUserFormModel
    {
        [Display(Name = "User name")]
        [Required]
        [StringLength(MaxUsernameLength,
            ErrorMessage = "{0} should be at between {2} and {1} letters",
            MinimumLength = MinUsernameLength)]
        public string UserName { get; set; }

        [Display(Name = "First name")]
        [StringLength(MaxLengthForNames,
            ErrorMessage = "{0} should be at least {2} letters",
            MinimumLength = MinLengthForNames)]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(MaxLengthForNames,
            ErrorMessage = "{0} should be at least {2} letters",
            MinimumLength = MinLengthForNames)]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Is email confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Is user admin")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Lockout end date")]
        public DateTime? LockoutEndDateUtc { get; set; }
    }
}