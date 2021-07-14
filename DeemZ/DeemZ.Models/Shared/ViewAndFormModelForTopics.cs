namespace DeemZ.Models.Shared
{ 
    using DeemZ.Models.ViewModels.Forum;
    using DeemZ.Models.FormModels.Forum;
    public class ViewAndFormModelForTopics
    {
        public TopicViewModel ViewModel { get; set; }
        public AddCommentFormModel FormModel { get; set; }
    }
}