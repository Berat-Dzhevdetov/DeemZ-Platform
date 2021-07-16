namespace DeemZ.Services.AdminServices
{
    using DeemZ.Models.ViewModels.Administration;
    public interface IAdminService
    {
        AdministrationIndexViewModel GetIndexPageInfo();
    }
}
