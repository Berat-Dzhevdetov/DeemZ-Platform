namespace DeemZ.Models.ViewModels.Administration
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;
    using DeemZ.Models.ViewModels.User;

    public class AdmistrationUsersViewModel : PagingBaseModel
    {
        public IEnumerable<BasicUserInformationViewModel> Users { get; set; }
        public string SearchTerm { get; set; }
    }
}
