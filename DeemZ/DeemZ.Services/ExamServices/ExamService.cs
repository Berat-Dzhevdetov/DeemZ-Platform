namespace DeemZ.Services.ExamServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Exam;
    using DeemZ.Data.Models;

    public class ExamService : IExamService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ExamService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void CreateExam(string cid, AddExamFormModel exam)
        {
            var newlyExam = mapper.Map<Exam>(exam);
            newlyExam.CourseId = cid;

            context.Exams.Add(newlyExam);
            context.SaveChanges();
        }

        public string EditExam(string examId, AddExamFormModel exam)
        {
            var examToEdit = GetExamById<Exam>(examId);

            examToEdit = mapper.Map<Exam>(exam);

            context.SaveChanges();

            return context.Exams.Where(x => x.Id == examId).Select(x => x.CourseId).FirstOrDefault();
        }

        public bool GetExamById(string eid)
            => context.Exams.Any(x => x.Id == eid);

        public T GetExamById<T>(string eid)
        {
            var exam = context.Exams.FirstOrDefault(x => x.Id == eid);

            return mapper.Map<T>(exam);
        }

        public IEnumerable<T> GetExamsByCourseId<T>(string cid)
            => context.Exams
                .Include(x => x.Questions)
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();
    }
}