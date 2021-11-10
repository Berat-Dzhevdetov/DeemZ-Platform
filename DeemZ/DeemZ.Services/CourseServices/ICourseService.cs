namespace DeemZ.Services.CourseServices
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.Course;

    public interface ICourseService
    {
        int GetUserCredits(string uid);
        IEnumerable<T> GetUserCurrentCourses<T>(string uid, bool isPaid = true);
        T GetCourseById<T>(string cid);
        bool GetCourseById(string cid);
        bool IsUserSignUpForThisCourse(string uid, string cid);
        IEnumerable<T> GetCoursesForSignUp<T>();
        Task SignUserToCourse(string uid, string cid, SignUpCourseFormModel signUp);
        Task SignUserToCourse(string uid, string cid, bool isPaid = true);
        IEnumerable<T> GetCourses<T>();
        int GetUserCoursesCount();
        Task<string> CreateCourse(AddCourseFormModel course);
        Task Edit(EditCourseFormModel course, string cid);
        Task DeleteCourse(string cid);
        void CreateBasicsLectures(string cid, AddCourseFormModel course);
        IEnumerable<T> GetUserCourses<T>(int page = 1, int quantity = 20);
        IEnumerable<KeyValuePair<string,string>> GetCourseByIdAsKeyValuePair(DateTime prevDate);
        Task DeleteUserFromCourse(string courseId, string userId);
        int UpCommingCoursesCount();
    }
}