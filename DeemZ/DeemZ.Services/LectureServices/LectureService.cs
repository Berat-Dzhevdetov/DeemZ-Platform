namespace DeemZ.Services.LectureServices
{
    using AutoMapper;
    using System.Linq;
    using DeemZ.Data;

    public class LectureService : ILectureService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public LectureService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public T GetLectureById<T>(string lid)
        {
            var lecture = context.Lectures.FirstOrDefault(x => x.Id == lid);

            return mapper.Map<T>(lecture);
        }
    }
}
