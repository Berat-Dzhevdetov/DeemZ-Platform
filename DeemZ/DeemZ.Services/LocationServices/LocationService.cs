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
                countryId = await CreateCountry(country);
            else
                countryId = await GetCountryId(country);

            if (!await AreaExists(region))
                areaId = await CreateArea(region, countryId);
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

        private async Task<string> CreateArea(string region, string countryId)
        {
            var newlyArea = new Area
            {
                Name = region,
                CountryId = countryId
            };

            context.Areas.Add(newlyArea);

            await context.SaveChangesAsync();

            return newlyArea.Id;
        }

        private async Task<bool> CityExists(string city)
            => await context.Cities.AnyAsync(x => x.Name == city);

        private async Task<bool> AreaExists(string region)
            => await context.Areas.AnyAsync(x => x.Name == region);

        public async Task<string> CreateCountry(string country)
        {
            var newlyCountry = new Country
            {
                Name = country
            };

            context.Countries.Add(newlyCountry);

            await context.SaveChangesAsync();
            return newlyCountry.Id;
        }

        public async Task<bool> LocationExists(string country, string region, string city)
            => await context.Cities.AnyAsync(x => x.Name == city && x.Area.Name == region && x.Area.Country.Name == country);

        private async Task<bool> CountryExists(string country)
            => await context.Countries.AnyAsync(x => x.Name == country);

        public async Task<string> GetCityId(string city)
            => (await context.Cities.FirstOrDefaultAsync(x => x.Name == city)).Id;
    }
}
