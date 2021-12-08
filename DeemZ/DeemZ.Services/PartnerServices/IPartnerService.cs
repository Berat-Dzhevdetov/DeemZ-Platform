namespace DeemZ.Services.PartnerServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeemZ.Models.FormModels.Partner;
    using DeemZ.Data.Models;

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
    }
}
