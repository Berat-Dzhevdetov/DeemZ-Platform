namespace DeemZ.Services.IpLoggServices
{
    using System.Threading.Tasks;

    public interface IIpLoggerService
    {
        Task<bool> IsTheSameIpAsLast(string uid, string newIp);
        Task SaveIpToUser(string userId, string ip, string city);
        bool IsNotFirstEnter(string userId);
    }
}
