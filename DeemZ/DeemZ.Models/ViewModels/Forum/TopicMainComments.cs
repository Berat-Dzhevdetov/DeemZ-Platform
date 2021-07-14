namespace DeemZ.Models.ViewModels.Forum
{
    using System.Collections.Generic;
    public class TopicMainComments : TopicBaseViewModel
    {
        public ICollection<TopicAnswerViewModel> Answers { get; set; }
    }
}