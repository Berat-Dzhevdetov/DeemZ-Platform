namespace DeemZ.Services.CourseServices
{
    using System.Collections.Generic;

    public interface ICourseService
    {
        int GetCreditsByUserId(string id);
        IEnumerable<T> GetCurrentCoursesByUserId<T>(string id,bool isPaid = true);
    }
}