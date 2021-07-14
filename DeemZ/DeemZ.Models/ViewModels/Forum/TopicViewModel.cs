namespace DeemZ.Models.ViewModels.Forum
{
    using System.Collections.Generic;
    public class TopicViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string UserProfileImg { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public ICollection<TopicCommentsViewModel> Comments { get; set; }
    }
}