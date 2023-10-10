using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext dataContext;

        public CountryRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task<bool> CountryExists(int id)
        {
            return await dataContext.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCountry(Country country)
        {
            await dataContext.Countries.AddAsync(country);
            return await Save();
        }

        public async Task<List<Country>> GetCountries()
        {
            return await dataContext.Countries.ToListAsync();
        }

        public async Task<Country> GetCountry(int id)
        {
            return await dataContext.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Country> GetCountryByOwner(int ownerId)
        {
            return await dataContext.Owners.Where(owner => owner.Id == ownerId).Select(c => c.Country).FirstOrDefaultAsync();            
        }

        public async Task<List<Owner>> GetOwnersFromACountry(int countryId)
        {
            return await dataContext.Owners.Where(owner => owner.Country.Id == countryId).ToListAsync();
        }

        public async Task<Country> HasCountry(string name)
        {
            var existringCountry = await dataContext.Countries.FirstOrDefaultAsync(c => c.Name.Trim().ToUpper() == name.Trim().ToUpper());
            //var existringCountry = await dataContext.Countries
            //.Where(c => c.Name.Trim().ToUpper() == name.TrimEnd().ToUpper()).FirstOrDefaultAsync();
            return existringCountry;
        }

        public async Task<bool> Save()
        {
            var saved = await dataContext.SaveChangesAsync();
            return saved > 0? true : false;
        }
    }
}
