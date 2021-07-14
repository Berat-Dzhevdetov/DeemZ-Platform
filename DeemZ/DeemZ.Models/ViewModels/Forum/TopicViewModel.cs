namespace DeemZ.Models.ViewModels.Forum
{
    using System.Collections.Generic;
    public class TopicViewModel : TopicBaseViewModel
    {
        public string Title { get; set; }
        public ICollection<TopicMainComments> Comments { get; set; }
    }
}