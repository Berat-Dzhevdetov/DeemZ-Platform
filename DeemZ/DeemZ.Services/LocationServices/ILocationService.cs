namespace DeemZ.Services.LocationServices
{
    using System.Threading.Tasks;

    public interface ILocationService
    {
        Task<bool> LocationExists(string country, string region, string city);
        Task CreateLocation(string country, string region, string city);
    }
}
