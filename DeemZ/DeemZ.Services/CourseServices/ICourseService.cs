namespace DeemZ.Services.CourseServices
{
    using DeemZ.Data.Models;
    using System.Collections.Generic;

    public interface ICourseService
    {
        int GetCreditsByUserId(string uid);
        IEnumerable<T> GetUserCurrentCourses<T>(string uid,bool isPaid = true);
        T GetCourseById<T>(string id);
        bool IsUserSignUpForThisCourse(string uid,string cid);
        IEnumerable<T> GetCoursesResources<T>(string uid);
    }
}