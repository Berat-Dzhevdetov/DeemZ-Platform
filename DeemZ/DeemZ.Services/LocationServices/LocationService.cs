namespace DeemZ.Services.LocationServices
{
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using DeemZ.Data;
    using DeemZ.Data.Models;

    public class LocationService : ILocationService
    {
        private readonly DeemZDbContext context;

        public LocationService(DeemZDbContext context)
        {
            this.context = context;
        }

        public async Task CreateLocation(string country, string region, string city)
        {
            string countryId = null;
            string areaId = null;

            if (!await CountryExists(country))
                await CreateCountry(country);
            else
                countryId = await GetCountryId(country);

            if (!await AreaExists(region))
                await CreateArea(region, countryId);
            else
                areaId = await GetAreaId(region);

            if (!await CityExists(city))
                await CreateCity(city, areaId);
        }

        private async Task<string> GetAreaId(string region)
            => (await context.Areas.FirstOrDefaultAsync(x => x.Name == region)).Id;

        private async Task<string> GetCountryId(string country)
            => (await context.Countries.FirstOrDefaultAsync(x => x.Name == country)).Id;

        private async Task CreateCity(string city, string areaId)
        {
            context.Cities.Add(new City
            {
                Name = city,
                AreaId = areaId
            });

            await context.SaveChangesAsync();
        }

        private async Task CreateArea(string region, string countryId)
        {
            context.Areas.Add(new Area
            {
                Name = region,
                CountryId = countryId
            });

            await context.SaveChangesAsync();
        }

        private Task<bool> CityExists(string city)
            => context.Cities.AnyAsync(x => x.Name == city);

        private Task<bool> AreaExists(string region)
            => context.Areas.AnyAsync(x => x.Name == region);

        public async Task CreateCountry(string country)
        {
            context.Countries.Add(new Country
            {
                Name = country
            });

            await context.SaveChangesAsync();
        }

        public Task<bool> LocationExists(string country, string region, string city)
            => context.Cities.AnyAsync(x => x.Name == city && x.Area.Name == region && x.Area.Country.Name == country);

        private Task<bool> CountryExists(string country)
            => context.Countries.AnyAsync(x => x.Name == country);
    }
}
