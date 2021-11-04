namespace DeemZ.Models.ViewModels.Surveys
{
    using System.Collections.Generic;
    using DeemZ.Models.Shared;

    public class MySurveyPagingViewModel : PagingBaseModel
    {
        public IEnumerable<MySurveyViewModel> Surveys { get; set; }
    }
}
