using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task<bool> CategoryExists(int id)
        {
            return await dataContext.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            await dataContext.Categories.AddAsync(category);
            return await Save();
        }

        public async Task<List<Category>> GetCategories()
        {
            return await dataContext.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Pokemon>> GetPokemonByCategory(int categoryId)
        {
            return await dataContext.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(p => p.Pokemon).ToListAsync();
        }

        public async Task<Category> HasCategory(string name)
        {
            var existingCategory = await dataContext.Categories
            .Where(c => c.Name.Trim().ToUpper() == name.TrimEnd().ToUpper()).FirstOrDefaultAsync();
            return existingCategory;
        }

        public async Task<bool> Save()
        {
            var saved = await dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
