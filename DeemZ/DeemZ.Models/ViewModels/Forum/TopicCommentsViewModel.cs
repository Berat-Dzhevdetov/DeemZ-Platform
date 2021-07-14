namespace DeemZ.Models.ViewModels.Forum
{
    using System.Collections.Generic;
    public class TopicCommentsViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string UserProfileImg { get; set; }
        public string Description { get; set; }
        public ICollection<TopicCommentsViewModel> Answers { get; set; }
    }
}