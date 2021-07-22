namespace DeemZ.Services.LectureServices
{
    using System.Collections.Generic;
    using DeemZ.Models.FormModels.Lecture;
    public interface ILectureService
    {
        T GetLectureById<T>(string lid);
        IEnumerable<T> GetLecturesByCourseId<T>(string cid);
        void AddLectureToCourse(string courseId,AddLectureFormModel lecture);
    }
}