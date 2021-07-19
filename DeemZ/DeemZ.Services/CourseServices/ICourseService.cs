namespace DeemZ.Services.CourseServices
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Course;

    public interface ICourseService
    {
        int GetUserCredits(string uid);
        IEnumerable<T> GetUserCurrentCourses<T>(string uid,bool isPaid = true);
        T GetCourseById<T>(string id);
        bool IsUserSignUpForThisCourse(string uid,string cid);
        IEnumerable<T> GetCoursesResources<T>(string uid);
        IEnumerable<T> GetCoursesForSignUp<T>();
        void SignUserToCourse(string uid, string cid);
        IEnumerable<T> GetCourses<T>();
        int GetUserCoursesCount();
        string CreateCourse(AddCourseFormModel course);
    }
}