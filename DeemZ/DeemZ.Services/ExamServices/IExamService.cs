namespace DeemZ.Services.ExamServices
{
    using System.Collections.Generic;
    public interface IExamService
    {
        IEnumerable<T> GetExamsByCourseId<T>(string courseId);
    }
}