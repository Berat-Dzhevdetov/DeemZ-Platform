namespace DeemZ.Services.FormService
{
    using DeemZ.Models.FormModels.Forum;

    public interface IForumService
    {
        void CreateTopic(CreateForumTopicFormModel model);
    }
}
