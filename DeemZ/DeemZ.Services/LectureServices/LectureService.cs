namespace DeemZ.Services.LectureServices
{
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Description;

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
                if (description.Name.Trim().Length < 3) continue;

                CreateDescription(description.Name, newlyLecture.Id);
            }

            context.Lectures.Add(newlyLecture);
            context.SaveChanges();
        }

        public IEnumerable<T> GetLectureResourcesById<T>(string lid)
            => context.Lectures
                .Where(x => x.Id == lid)
                .Include(x => x.Resources)
                .Select(x => mapper.Map<T>(x))
                .ToList();

        public void EditLectureById(string lectureId, EditLectureFormModel lecture)
        {
            var lectureToEdit = GetLectureById<Lecture>(lectureId);

            lectureToEdit.Name = lecture.Name;
            lectureToEdit.Date = lecture.Date;

            for (int i = 0; i < lecture.Descriptions.Count; i++)
            {
                var (id, name) = GetDescriptionIdAndName(lecture.Descriptions[i]);
                var description = GetDescriptionById(id);
                if (description == null)
                {
                    if (name.Trim().Length < 3) continue;

                    description = CreateDescription(name , lectureId);
                }
                description.Name = name;
                context.SaveChanges();
            }

            context.SaveChanges();
        }

        private Description CreateDescription(string name, string lectureId)
        {
            var descripiton = new Description()
            {
                Name = name,
                LectureId = lectureId
            };

            context.Descriptions.Add(descripiton);

            context.SaveChanges();
            return descripiton;
        }

        private (string id,string name) GetDescriptionIdAndName(EditDescriptionFormModel model)
        {
            var id = model.Id;
            var name = model.Name;
            return (id, name);
        }

        private Description GetDescriptionById(string did)
            => context.Descriptions.Find(did);

        public T GetLectureById<T>(string lid)
        {
            var lecture = context.Lectures
                .FirstOrDefault(x => x.Id == lid);

            return mapper.Map<T>(lecture);
        }

        public bool GetLectureById(string lid)
            => context.Lectures.Any(x => x.Id == lid);

        public List<T> GetLectureDescriptions<T>(string lid)
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

        public void DeleteDescription(string did)
        {
            var description = GetDescriptionById(did);

            if (description == null) return;

            context.Descriptions.Remove(description);
            context.SaveChanges();
        }
    }
}
