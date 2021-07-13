namespace DeemZ.Services.ForumService
{
    using AutoMapper;
    using System.Collections.Generic;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class ForumService : IForumService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ForumService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string CreateTopic(CreateForumTopicFormModel model, string uid)
        {
            var entity = mapper.Map<Forum>(model);
            entity.UserId = uid;

            context.Forums.Add(entity);
            context.SaveChanges();

            return entity.Id;
        }

        public IEnumerable<T> GetAllTopics<T>()
            => context.Forums
                .Include(x => x.User)
                .OrderBy(x => x.CreatedOn)
                .Select(x => mapper.Map<T>(x))
                .ToList();

        public IEnumerable<T> GetAllTopics<T>(int page = 1, int quantity = 10)
            => GetAllTopics<T>()
                .Skip(page - 1 * quantity)
                .Take(quantity)
                .ToList();

        public T GetTopicById<T>(string tid)
        {
            var topic = context.Forums
                .FirstOrDefault(x => x.Id == tid);

            return mapper.Map<T>(topic);
        }

        public int Count() => context.Forums.Count();
    }
}
