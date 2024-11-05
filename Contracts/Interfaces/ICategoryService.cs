using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface ICategoryService
    {
        public string AddCategory(Category category);
        public string DeleteCategory(Category category);
        public string UpdateCategory(Category category);
        public List<Category> GetAllCategories();
        public List<Category> GetRootCategories();
        public List<Category> GetCategoriesByParent(string categoryId);
    }
}
