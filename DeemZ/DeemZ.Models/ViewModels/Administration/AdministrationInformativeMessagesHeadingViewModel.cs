namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.InformativeMessages;

    public class AdministrationInformativeMessagesHeadingViewModel : PagingBaseModel
    {
        public IEnumerable<InformativeMessagesHeadingViewModel> InformativeMessagesHeadingViewModel { get; set; }
    }
}
