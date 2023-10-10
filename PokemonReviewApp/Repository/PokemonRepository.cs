using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext dataContext;

        public PokemonRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var existingOwner = await dataContext.Owners.FirstOrDefaultAsync(o => o.Id == ownerId);
            var existingCategory = await dataContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            var pokemonOwnerEntity = new PokemonOwner
            {
                Pokemon = pokemon,
                Owner = existingOwner
            };

            var pokemonCategoryEntity = new PokemonCategory
            {
                Pokemon = pokemon,
                Category = existingCategory
            };

            await dataContext.Pokemon.AddAsync(pokemon);
            await dataContext.PokemonOwners.AddAsync(pokemonOwnerEntity);
            await dataContext.PokemonCategories.AddAsync(pokemonCategoryEntity);

            return await Save();
        }

        public async Task<Pokemon> GetPokemon(int id)
        {
            var pokemon = await dataContext.Pokemon.FirstOrDefaultAsync(p => p.Id == id);
            return pokemon;
        }

        public async Task<Pokemon> GetPokemon(string name)
        {
            var pokemon = await dataContext.Pokemon.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
            return pokemon;
        }

        public async Task<decimal> GetPokemonRating(int pokeId)
        {
            var reviews = await dataContext.Reviews.Where(r => r.Pokemon.Id == pokeId).ToListAsync();
            if(reviews.Count == 0)
            {
                return 0;
            }
            return (decimal)reviews.Sum(r => r.Rating) / reviews.Count;
        }

        public async Task<List<Pokemon>> GetPokemons()
        {
            return await dataContext.Pokemon.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Pokemon> HasPokemon(string name)
        {
            var existingPokemon = await dataContext.Pokemon.FirstOrDefaultAsync(p => p.Name.ToUpper() == name.Trim().ToUpper());
            return existingPokemon;
        }

        public bool PokemonExists(int pokeId)
        {
            return dataContext.Pokemon.Any(p => p.Id == pokeId);
        }

        public async Task<bool> Save()
        {
            var saved = await dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
