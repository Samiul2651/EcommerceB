using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface ICategoryService
    {
        public Task<string> AddCategory(Category category);
        public Task<string> DeleteCategory(Category category);
        public Task<string> UpdateCategory(Category category);
        public Task<List<Category>> GetAllCategories();
        public Task<List<Category>> GetRootCategories();
        public Task<List<Category>> GetCategoriesByParent(string categoryId);
        public Task<List<Category>> GetCategoriesBySearch(string input);
        public Task<Category> GetCategoryById(string id);
        public Task<List<Category>> GetTrendingCategories();
        public Task AddTrendingScore(string categoryId, int value);
    }
}
