namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.InformativeMessages;

    public class AdministrationInformativeMessages : PagingBaseModel
    {
        public IEnumerable<InformativeMessageDetailsViewModel> InformativeMessages { get; set; }

        public string Id { get; set; }
    }
}
