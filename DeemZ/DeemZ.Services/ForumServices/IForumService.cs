namespace DeemZ.Services.ForumServices
{
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Forum;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IForumService
    {
        Task<string> CreateTopic(CreateForumTopicFormModel model, string uid);
        IEnumerable<T> GetAllTopics<T>();
        IEnumerable<T> GetAllTopics<T>(int page = 1, int quantity = 10);
        Task<T> GetTopicById<T>(string tid);
        int Count();
        IEnumerable<T> GetTopicsByTitleName<T>(string title, int page = 1, int quantity = 10);
        Task<string> CreateComment(AddCommentFormModel model, string tid, string uid);
        Comment GetCommentById(string cid);
    }
}