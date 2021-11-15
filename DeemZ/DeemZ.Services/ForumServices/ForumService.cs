namespace DeemZ.Services.ForumServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using DeemZ.Global.Extensions;
    using System.Threading.Tasks;

    public class ForumService : IForumService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ForumService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<string> CreateTopic(CreateForumTopicFormModel model, string uid)
        {
            var newlyTopic = mapper.Map<Forum>(model);
            newlyTopic.UserId = uid;

            newlyTopic.CreatedOn = newlyTopic.CreatedOn.ToUniversalTime();

            context.Forums.Add(newlyTopic);
            await context.SaveChangesAsync();

            return newlyTopic.Id;
        }

        public IEnumerable<T> GetAllTopics<T>()
            => context.Forums
                .Include(x => x.User)
                .OrderBy(x => x.CreatedOn)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<T> GetAllTopics<T>(int page = 1, int quantity = 10)
            => GetAllTopics<T>()
                .Paging(page, quantity)
                .ToList();

        public T GetTopicById<T>(string tid)
        {
            var topic = context.Forums
                .Include(x => x.Comments)
                .Include(x => x.User)
                .OrderBy(x => x.CreatedOn)
                .FirstOrDefault(x => x.Id == tid);

            return mapper.Map<T>(topic);
        }

        public int Count() => context.Forums.Count();

        public IEnumerable<T> GetTopicsByTitleName<T>(string title, int page = 1, int quantity = 10)
            => context.Forums
                .Include(x => x.User)
                .Where(x => x.Title.ToLower().Contains(title.ToLower()))
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public async Task<string> CreateComment(AddCommentFormModel model, string tid, string uid)
        {
            var comment = mapper.Map<Comment>(model);

            comment.UserId = uid;
            comment.ForumId = tid;

            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            return comment.Id;
        }

        public Comment GetCommentById(string cid)
            => context.Comments.FirstOrDefault(x => x.Id == cid);
    }
}