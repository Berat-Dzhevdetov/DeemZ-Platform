namespace DeemZ.Services.AdminServices
{
    using System.Collections.Generic;
    using DeemZ.Models.ViewModels.Administration;

    public interface IAdminService
    {
        AdministrationIndexViewModel GetIndexPageInfo();
        IEnumerable<T> GetUserCourses<T>(int page = 1, int quantity = 20);
        int GetUserCoursesCount();
        int GetUserSignUpForCourse(string cid);
        int GetUserSignedUpForCourse(string cid);
    }
}
