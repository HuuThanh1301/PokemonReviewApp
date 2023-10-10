using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        Task<List<Reviewer>> GetReviewers();
        Task<Reviewer> GetReviewer(int reviewerId);
        Task<List<Review>> GetReviewsByReviewer(int reviewerId);
        Task<bool> ReviewerExists(int reviewerId);
    }
}
