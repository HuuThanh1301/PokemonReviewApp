using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviews();
        Task<Review> GetReview(int reviewId);
        Task<List<Review>> GetReviewsOfAPokemon(int pokeId);
        Task<bool> ReviewExists(int reviewId);
        Task<bool> CreateReview(Review review);
        Task<bool> Save();
        Task<Review> HasReview(string title);
    }
}
