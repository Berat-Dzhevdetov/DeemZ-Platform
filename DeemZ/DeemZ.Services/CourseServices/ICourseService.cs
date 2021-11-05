namespace DeemZ.Services.CourseServices
{
    using System;
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Course;

    public interface ICourseService
    {
        int GetUserCredits(string uid);
        IEnumerable<T> GetUserCurrentCourses<T>(string uid, bool isPaid = true);
        T GetCourseById<T>(string cid);
        bool GetCourseById(string cid);
        bool IsUserSignUpForThisCourse(string uid, string cid);
        IEnumerable<T> GetCoursesForSignUp<T>();
        void SignUserToCourse(string uid, string cid, SignUpCourseFormModel signUp);
        void SignUserToCourse(string uid, string cid, bool isPaid = true);
        IEnumerable<T> GetCourses<T>();
        int GetUserCoursesCount();
        string CreateCourse(AddCourseFormModel course);
        void Edit(EditCourseFormModel course, string cid);
        void DeleteCourse(string cid);
        void CreateBasicsLectures(string cid, AddCourseFormModel course);
        IEnumerable<T> GetUserCourses<T>(int page = 1, int quantity = 20);
        IEnumerable<KeyValuePair<string,string>> GetCourseByIdAsKeyValuePair(DateTime prevDate);
        void DeleteUserFromCourse(string courseId, string userId);
        int UpCommingCoursesCount();
    }
}