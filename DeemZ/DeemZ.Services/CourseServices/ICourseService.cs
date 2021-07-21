namespace DeemZ.Services.CourseServices
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Course;

    public interface ICourseService
    {
        int GetUserCredits(string uid);
        IEnumerable<T> GetUserCurrentCourses<T>(string uid,bool isPaid = true);
        T GetCourseById<T>(string id);
        bool GetCourseById(string id);
        bool IsUserSignUpForThisCourse(string uid,string cid);
        IEnumerable<T> GetCoursesForSignUp<T>();
        void SignUserToCourse(string uid, string cid);
        IEnumerable<T> GetCourses<T>();
        int GetUserCoursesCount();
        string CreateCourse(AddCourseFormModel course);
        void EditCourseById(EditCourseFormModel course, string courseId);
        IEnumerable<T> GetLectureResourcesById<T>(string lid);
    }
}