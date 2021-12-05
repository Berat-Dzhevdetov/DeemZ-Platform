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

    public class PartnerService : IPartnerService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public PartnerService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

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
                    .Paging(page,quantity)
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