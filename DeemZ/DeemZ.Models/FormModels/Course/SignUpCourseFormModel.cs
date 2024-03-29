﻿namespace DeemZ.Models.FormModels.Course
{
    using System.ComponentModel.DataAnnotations;

    using static DeemZ.Data.DataConstants.PromoCode;

    public class SignUpCourseFormModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(16,MinimumLength = 8,ErrorMessage = "{0} number should be between {2} and {1} digits")]
        [Display(Name = "Credit card")]
        [RegularExpression(@"^[0-9]{8,16}$",ErrorMessage = "{0} number must be digits only")]
        public string CreditCard { get; set; }
        [Required]
        [Display(Name ="Course name")]
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        [Display(Name ="Promo code")]
        [StringLength(TextLength, MinimumLength = TextLength)]
        public string PromoCode { get; set; }
    }
}