using System.ComponentModel.DataAnnotations;

namespace DeemZ.Models.FormModels.Course
{
    public class SignUpCourseFormModel
    {
        [Required]
        [StringLength(16,MinimumLength = 8,ErrorMessage = "{0} number should be between {2} and {1} numbers")]
        [Display(Name = "Credit card")]
        [RegularExpression(@"^[0-9]{8,16}$",ErrorMessage = "{0} number must be digits only")]
        public string CreditCard { get; set; }
    }
}
