namespace DeemZ.Services.LectureServices
{
    using System.Collections.Generic;
    public interface ILectureService
    {
        T GetLectureById<T>(string lid);
        IEnumerable<T> GetLecturesByCourseId<T>(string cid);
    }
}