namespace DeemZ.Services.CourseServices
{
    using System.Collections.Generic;
    using DeemZ.Data.Models;

    public interface ICourseService
    {
        int GetUserCredits(string uid);
        IEnumerable<T> GetUserCurrentCourses<T>(string uid,bool isPaid = true);
        T GetCourseById<T>(string id);
        bool IsUserSignUpForThisCourse(string uid,string cid);
        IEnumerable<T> GetCoursesResources<T>(string uid);
        IEnumerable<T> GetCoursesForSignUp<T>();
        Course DoesTheCourseExist(string cid);
        bool SignUserToCourse(string uid, string cid);
    }
}