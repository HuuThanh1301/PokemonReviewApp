using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountries();
        Task<Country> GetCountry(int id);
        Task<Country> GetCountryByOwner(int ownerId);
        Task<List<Owner>> GetOwnersFromACountry(int countryId);
        Task<bool> CountryExists(int id);
        Task<bool> CreateCountry(Country country);
        Task<bool> Save();
        Task<Country> HasCountry(string name);

    }
}
