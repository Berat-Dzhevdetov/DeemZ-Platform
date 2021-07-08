namespace DeemZ.Services.Course
{
    using System.Collections.Generic;
    public interface ICourseService
    {
        int GetCreditsByUserId(string id);
        IEnumerable<IndexCourseViewModel> GetCoursesByUserId(string id);
    }
}