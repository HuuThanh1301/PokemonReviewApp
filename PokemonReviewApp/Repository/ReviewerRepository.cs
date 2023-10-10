using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext dataContext;

        public ReviewerRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }



        public async Task<Reviewer> GetReviewer(int reviewerId)
        {
            return await dataContext.Reviewers.FirstOrDefaultAsync(r => r.Id == reviewerId);
        }

        public async Task<List<Reviewer>> GetReviewers()
        {
            return await dataContext.Reviewers.ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByReviewer(int reviewerId)
        {
            return await dataContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToListAsync();
        }



        public async Task<bool> ReviewerExists(int reviewerId)
        {
            return await dataContext.Reviewers.AnyAsync(r => r.Id == reviewerId);
        }


    }
}
