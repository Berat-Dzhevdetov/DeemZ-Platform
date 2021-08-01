namespace DeemZ.Services.ExamServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using DeemZ.Data;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
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

        public void CreateExam(string courseId, AddExamFormModel exam)
        {
            var newlyExam = mapper.Map<Exam>(exam);
            newlyExam.CourseId = courseId;

            context.Exams.Add(newlyExam);
            context.SaveChanges();
        }

        public IEnumerable<T> GetExamsByCourseId<T>(string cid)
            => context.Exams
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();
    }
}