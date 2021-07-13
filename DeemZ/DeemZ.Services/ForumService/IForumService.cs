namespace DeemZ.Services.ForumService
{
    using DeemZ.Models.FormModels.Forum;
    using System.Collections.Generic;

    public interface IForumService
    {
        string CreateTopic(CreateForumTopicFormModel model, string uid);
        IEnumerable<T> GetAllTopics<T>();
        IEnumerable<T> GetAllTopics<T>(int page = 1, int quantity = 10);
        T GetTopicById<T>(string tid);
        int Count();
    }
}
