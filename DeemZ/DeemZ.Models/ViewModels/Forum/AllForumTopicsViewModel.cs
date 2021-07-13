namespace DeemZ.Models.ViewModels.Forum
{
    using System.Collections.Generic;
    public class AllForumTopicsViewModel
    {
        public int CurrentPage { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }

        public IEnumerable<ForumTopicsViewModel> Topics { get; set; }
    }
}
