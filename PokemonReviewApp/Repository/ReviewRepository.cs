using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext dataContext;

        public ReviewRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Review> GetReview(int reviewId)
        {
            return await dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task<List<Review>> GetReviews()
        {
            return await dataContext.Reviews.ToListAsync();
        }

        public async Task<List<Review>> GetReviewsOfAPokemon(int pokeId)
        {
            return await dataContext.Reviews.Where(r => r.Pokemon.Id == pokeId).ToListAsync();
        }

        public async Task<bool> ReviewExists(int reviewId)
        {
            return await dataContext.Reviews.AnyAsync(r => r.Id == reviewId);
        }
        public async Task<bool> Save()
        {
            var saved = await dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
        public async Task<Review> HasReview(string title)
        {
            var exisistingReview = await dataContext.Reviews.FirstOrDefaultAsync(r => r.Title.ToUpper() == title.Trim().ToUpper());
            return exisistingReview;
        }
        public async Task<bool> CreateReview(Review review)
        {
            await dataContext.AddAsync(review);
            return await Save();
        }
    }
}
