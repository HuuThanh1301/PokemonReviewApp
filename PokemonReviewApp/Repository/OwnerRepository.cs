using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext dataContext;

        public OwnerRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task<Owner> GetOwner(int ownerId)
        {
            return await dataContext.Owners.FirstOrDefaultAsync(o => o.Id == ownerId);
        }

        public async Task<List<Owner>> GetOwnersOfAPokemon(int pokeId)
        {
            return await dataContext.PokemonOwners.Where(po => po.PokemonId == pokeId).Select(p => p.Owner).ToListAsync();
        }

        public async Task<List<Owner>> GetOwners()
        {
            return await dataContext.Owners.ToListAsync();
        }

        public async Task<List<Pokemon>> GetPokemonByOwner(int ownerId)
        {
            return await dataContext.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(p => p.Pokemon).ToListAsync();
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            return await dataContext.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> CreateOwner(Owner owner)
        {
            await dataContext.Owners.AddAsync(owner);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<Owner> HasOwner(string fname, string lname)
        {
            var existingOwner = await dataContext.Owners.
                FirstOrDefaultAsync(o => o.FirstName.Trim().ToUpper() + " " + o.LastName.Trim().ToUpper() == fname.Trim().ToUpper()+ " " + lname.Trim().ToUpper());
            return existingOwner;
        }
    }
}
