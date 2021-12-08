namespace DeemZ.Services.PartnerServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using Data;
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Global.Extensions;
    using DeemZ.Models.FormModels.Partner;
    using DeemZ.Data.Models;
    using DeemZ.Services.FileService;
    using DeemZ.Models.ViewModels.Partners;

    public class PartnerService : IPartnerService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly string secretKey = "partner";

        public PartnerService(DeemZDbContext context, IMapper mapper, IFileService fileService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task Create(AddPartnerFormModel partner)
        {
            var newPartner = mapper.Map<Partner>(partner);

            newPartner.From = DateTime.UtcNow;

            (newPartner.Path, newPartner.PublicId) = fileService.PreparingFileForUploadAndUploadIt(partner.LogoImage, secretKey);

            if (newPartner.Path == null || newPartner.PublicId == null)
                return;

            context.Partners.Add(newPartner);

            await context.SaveChangesAsync();
        }

        public async Task Delete(string pid)
        {
            var partnerToDel = await context.Partners.FirstOrDefaultAsync(x => x.Id == pid);

            fileService.DeleteFile(partnerToDel.PublicId, isImg: true);

            context.Remove(partnerToDel);

            await context.SaveChangesAsync();
        }

        public async Task Edit(string partnerId, EditPartnerFormModel formModel)
        {
            var partnerToEdit = await context.Partners.FirstOrDefaultAsync(x => x.Id == partnerId);

            partnerToEdit.Name = formModel.Name;
            partnerToEdit.Tier = formModel.Tier;

            if (formModel.IsImageChanged)
            {
                fileService.DeleteFile(partnerToEdit.PublicId, isImg: true);

                fileService.PreparingFileForUploadAndUploadIt(formModel.LogoImage, secretKey);
            }

            await context.SaveChangesAsync();
        }

        public List<IGrouping<PartnerTiers, PartnersDetailsViewModel>> GetAllPartners()
            => context.Partners
                .ProjectTo<PartnersDetailsViewModel>(mapper.ConfigurationProvider)
            .ToList()
                .GroupBy(x => x.Tier)
                .ToList();

        public async Task<T> GetPartnerById<T>(string partnerId)
            => await context.Partners.Where(x => x.Id == partnerId).ProjectTo<T>(mapper.ConfigurationProvider).FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetPartners<T>(int? tier, string name, int page = 1, int quantity = 20)
            => await Task.Run(async () =>
            {
                var allPartners = context.Partners
                    .OrderByDescending(x => x.From)
                    .AsQueryable();

                if (tier != null)
                {
                    if (await ValidateTier((int)tier))
                    {
                        allPartners = allPartners.Where(x => (int)x.Tier == tier).AsQueryable();
                    }
                }

                if (!string.IsNullOrWhiteSpace(name))
                    allPartners = allPartners.Where(x => x.Name == name).AsQueryable();

                return allPartners
                    .ProjectTo<T>(mapper.ConfigurationProvider)
                    .Paging(page, quantity)
                    .ToList();
            });

        public async Task<int> GetPartnersCount()
            => await context.Partners.CountAsync();

        public async Task<IDictionary<int, string>> GetTiers()
            => await Task.Run(() =>
            {
                var keyValuePair = new Dictionary<int, string>();

                foreach (var tier in Enum.GetValues(typeof(PartnerTiers)))
                {
                    keyValuePair.Add((int)tier, tier.ToString());
                }

                return keyValuePair;
            });

        public async Task<bool> ValidateTier(int tier)
            => await Task.Run(() =>
            {
                if (Enum.IsDefined(typeof(PartnerTiers), tier))
                    return true;
                return false;
            });
    }
}