namespace DeemZ.Models.ViewModels.Forum
{
    using System;
    public class ForumTopicsViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserProfileImgUrl { get; set; }
        public string Username { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}