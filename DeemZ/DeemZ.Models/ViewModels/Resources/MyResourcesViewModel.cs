namespace DeemZ.Models.ViewModels.Resources
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;

    public class MyResourcesViewModel : PagingBaseModel
    {
        public IEnumerable<DetailsResourseViewModel> Resources { get; set; } = new HashSet<DetailsResourseViewModel>();
    }
}
