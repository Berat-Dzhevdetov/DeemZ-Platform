namespace DeemZ.Services.IpLoggServices
{
    using DeemZ.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class IpLoggerService : IIpLoggerService
    {
        private readonly DeemZDbContext context;

        public IpLoggerService(DeemZDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> IsTheSameIpAsLast(string uid, string newIp)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == uid);
            return user.Loggs.Count > 0 && user.Loggs.Last().Ip == newIp;
        }
    }
}
