namespace DeemZ.Services.ExamServices
{
    using DeemZ.Models.FormModels.Exam;
    using System.Collections.Generic;
    public interface IExamService
    {
        IEnumerable<T> GetExamsByCourseId<T>(string courseId);
        void CreateExam(string courseId, AddExamFormModel exam);
    }
}