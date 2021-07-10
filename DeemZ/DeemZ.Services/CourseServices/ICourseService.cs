namespace DeemZ.Services.CourseServices
{
    using System.Collections.Generic;

    public interface ICourseService
    {
        int GetCreditsByUserId(string id);
        IEnumerable<T> GetUserCurrentCourses<T>(string id,bool isPaid = true);
        T GetCourseById<T>(string id);
        bool IsUserSignUpForThisCourse(string uid,string cid);
    }
}