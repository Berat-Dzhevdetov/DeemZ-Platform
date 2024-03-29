﻿namespace DeemZ.Data.Models
{
    using System;
    public class UserCourse
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Course Course { get; set; }
        public string CourseId { get; set; }
        public DateTime? PaidOn { get; set; }
        public bool IsPaid { get; set; }
        public decimal Paid { get; set; }
        public PromoCode PromoCode { get; set; }
        public string PromoCodeId { get; set; }
    }
}
