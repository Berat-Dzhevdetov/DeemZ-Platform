namespace DeemZ.Services.PartnerServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Partner;
    using DeemZ.Models.ViewModels.Partners;

    public interface IPartnerService
    {
        Task<IEnumerable<T>> GetPartners<T>(int? tier, string name, int page = 1, int quantity = 20);
        Task<int> GetPartnersCount();
        Task<IDictionary<int, string>> GetTiers();
        Task<bool> ValidateTier(int tier);
        Task Create(AddPartnerFormModel partner);
        Task Delete(string pid);
        Task Edit(string partnerId, EditPartnerFormModel formModel);
        Task<T> GetPartnerById<T>(string partnerId);
        List<IGrouping<PartnerTiers, PartnersDetailsViewModel>> GetAllPartners();
    }
}
