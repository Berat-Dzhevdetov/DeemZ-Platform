namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    public class AdministrationIndexViewModel
    {
        public int UsersCount { get; set; }
        public decimal MoneyEarnedThisMonth { get; set; }
        public int TotalCourses { get; set; }
        public int UsersSignedUpThisMonth { get; set; }
        public IEnumerable<UserCoursesViewModel> UserCourses { get; set; }

        public int CurrentPage { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }
        public int MaxPages { get; set; }
    }
}