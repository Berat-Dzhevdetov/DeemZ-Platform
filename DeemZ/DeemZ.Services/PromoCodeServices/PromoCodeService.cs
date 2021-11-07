namespace DeemZ.Services.PromoCodeServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Global.Extensions;
    using DeemZ.Models.FormModels.PromoCode;
    using DeemZ.Services.UserServices;

    using static DeemZ.Data.DataConstants.PromoCode;

    public class PromoCodeService : IPromoCodeService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly Random random;

        public PromoCodeService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.random = new Random();
        }

        public async Task<string> GeneratePromoCodeText()
        {
            var task = Task.Run(() =>
            {
                string promoCode;
                do
                {
                    promoCode = RandomString(TextLength);
                } while (IfExists(promoCode));
                return promoCode;
            });

            return await task;
        }

        public bool IfExists(string promoCode)
            => context.PromoCodes
                .Any(x => x.Text == promoCode);

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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

        public void AddPromoCode(AddPromoCodeFormModel promoCode)
        {
            var userId = context.Users.FirstOrDefault(x => x.UserName == promoCode.UserName).Id;
            
            var newPromoCode = new PromoCode()
            {
                Text = promoCode.Text,
                DiscountPrice = promoCode.DiscountPrice,
                ExpireOn = promoCode.ExpireOn.ToUniversalTime(),
                IsUsed = false,
                ApplicationUserId = userId
            };

            context.PromoCodes.Add(newPromoCode);

            context.SaveChanges();
        }
    }
}
