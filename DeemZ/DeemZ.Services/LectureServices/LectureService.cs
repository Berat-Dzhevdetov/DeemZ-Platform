namespace DeemZ.Services.LectureServices
{
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using DeemZ.Data;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Data.Models;

    public class LectureService : ILectureService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public LectureService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddLectureToCourse(string courseId, AddLectureFormModel lecture)
        {
            var newlyLecture = mapper.Map<Lecture>(lecture);
            newlyLecture.CourseId = courseId;

            foreach (var description in lecture.Descriptions)
            {
                var newlyDescription = mapper.Map<Description>(description);
                newlyDescription.LectureId = newlyLecture.Id;
            }

            context.Lectures.Add(newlyLecture);
            context.SaveChanges();
        }

        public T GetLectureById<T>(string lid)
        {
            var lecture = context.Lectures
                .FirstOrDefault(x => x.Id == lid);

            return mapper.Map<T>(lecture);
        }

        public bool GetLectureById(string lid)
            => context.Lectures.Any(x => x.Id == lid);

        public IEnumerable<T> GetLectureDescriptions<T>(string lid)
            => context.Descriptions
                .Where(x => x.LectureId == lid)
                .Select(x => mapper.Map<T>(x))
                .ToList();

        public IEnumerable<T> GetLecturesByCourseId<T>(string cid)
            => context.Lectures
                .Include(x => x.Course)
                .Where(x => x.CourseId == cid)
                .Select(x => mapper.Map<T>(x))
                .ToList();
    }
}
