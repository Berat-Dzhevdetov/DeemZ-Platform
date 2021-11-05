namespace DeemZ.Services.PromoCodeServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Global.Extensions;

    public class PromoCodeService : IPromoCodeService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public PromoCodeService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public PromoCode GetPromoCode(string promoCode)
            => context.PromoCodes.FirstOrDefault(x => x.Text == promoCode);

        public IEnumerable<T> GetPromoCodes<T>(string promoCode)
            => context.PromoCodes
                .Include(x => x.ApplicationUser)
                .OrderByDescending(x => x.ExpireOn)
                .Where(x => x.Text.Contains(promoCode))
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<T> GetPromoCodes<T>(int page = 1, int quantity = 20)
            => context.PromoCodes
                .Include(x => x.ApplicationUser)
                .OrderByDescending(x => x.ExpireOn)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public int GetPromoCodesCount()
            => context.PromoCodes
                .Count();

        public void MarkPromoCodeAsUsed(string pcid)
        {
            var promoCode = context.PromoCodes.FirstOrDefault(x => x.Id == pcid);

            promoCode.IsUsed = true;

            context.SaveChanges();
        }

        public bool ValidatePromoCode(string uid, string promoCode)
            => context.PromoCodes
                .Any(x => x.ApplicationUserId == uid
                        && !x.IsUsed && x.Text == promoCode);
    }
}
