namespace DeemZ.Services.Course
{
    using DeemZ.Models.ViewModels.Course;
    using System.Collections.Generic;
    public interface ICourseService
    {
        int GetCreditsByUserId(string id);
        IEnumerable<IndexCourseViewModel> GetCurrentCoursesByUserId(string id);
    }
}