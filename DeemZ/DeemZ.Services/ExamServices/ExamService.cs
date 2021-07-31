namespace DeemZ.Services.ExamServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using DeemZ.Data;
    using System.Linq;
    using AutoMapper.QueryableExtensions;

    public class ExamService : IExamService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ExamService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<T> GetExamsByCourseId<T>(string cid)
            => context.Exams
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();
    }
}