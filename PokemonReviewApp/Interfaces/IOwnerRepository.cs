using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        Task<List<Owner>> GetOwners();
        Task<Owner> GetOwner(int ownerId);
        Task<List<Owner>> GetOwnersOfAPokemon(int pokeId);
        Task <List<Pokemon>> GetPokemonByOwner(int ownerId);
        Task<bool> OwnerExists(int ownerId);
        Task<bool> CreateOwner(Owner owner);
        Task<bool> Save();
        Task<Owner> HasOwner(string fname,string lname);

    }
}
