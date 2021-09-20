namespace DeemZ.Models.ViewModels.InformativeMessages
{
    using System.Collections.Generic;

    public class InformativeMessagesHeadingViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<InformativeMessageViewModel> InformativeMessages { get; set; } = new List<InformativeMessageViewModel>();
    }
}
