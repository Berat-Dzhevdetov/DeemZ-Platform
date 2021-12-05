namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.Partners;

    public class AdministrationPartnersViewModel : PagingBaseModel
    {
        public IEnumerable<PartnersDetailsViewModel> Partners { get; set; }
        public IDictionary<int,string> Tiers { get; set; }
        public int Tier { get; set; }
        public string Name { get; set; }
    }
}
