namespace DeemZ.Services.IpLoggServices
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Services.LocationServices;

    public class IpLoggerService : IIpLoggerService
    {
        private readonly DeemZDbContext context;
        private readonly ILocationService locationService;

        public IpLoggerService(DeemZDbContext context, ILocationService locationService)
        {
            this.context = context;
            this.locationService = locationService;
        }

        public bool IsNotFirstEnter(string userId)
            => context.Users.Include(x => x.Loggs).FirstOrDefault(x => x.Id == userId).Loggs.Count > 0;

        public async Task<bool> IsTheSameIpAsLast(string uid, string newIp)
        {
            var user = await context.Users
                    .Include(x => x.Loggs)
                    .FirstOrDefaultAsync(x => x.Id == uid);
            return user.Loggs.OrderByDescending(x => x.CreatedOn).Last().Ip != newIp;
        }

        public async Task SaveIpToUser(string userId, string ip, string city)
        {
            var cityId = await locationService.GetCityId(city);

            var ipLogg = new IpLogg()
            {
                CityId = cityId,
                Ip = ip,
                UserId = userId,
            };

            context.IpLoggs.Attach(ipLogg);
            context.IpLoggs.Add(ipLogg);
            await context.SaveChangesAsync();
        }
    }
}
