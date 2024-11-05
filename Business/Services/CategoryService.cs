using Contracts.Constants;
using Contracts.Interfaces;
using Contracts.Models;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoDbService _mongoDbService;

        public CategoryService(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public string AddCategory(Category newCategory)
        {
            bool Filter(Category category) => category.Name == newCategory.Name;
            var checkCategory = _mongoDbService.GetObjectByFilter(nameof(Category), (Func<Category, bool>)Filter);
            if (checkCategory != null && checkCategory.Name == newCategory.Name)
            {
                return UpdateStatus.BadRequest;
            }
            try
            {
                _mongoDbService.AddObject(nameof(Category), newCategory);
                return UpdateStatus.Success;
            }
            catch (Exception ex)
            {
                return UpdateStatus.Failed;
            }
        }

        public string DeleteCategory(Category category)
        {
            
            bool result = _mongoDbService.DeleteObject<Category>(nameof(Category), category.Id);
            if (!result)
            {
                return UpdateStatus.BadRequest;
            }
            bool Filter(Product product) => product.Category == category.Id;
            var productsByCategory = _mongoDbService.GetListByFilter(nameof(Product), (Func<Product, bool>)Filter);
            foreach (var product in productsByCategory)
            {
                product.Category = "";
                _mongoDbService.UpdateObject(nameof(Product), product);
            }
            return UpdateStatus.Success;
        }

        public string UpdateCategory(Category category)
        {
            var categoryToBeUpdated = _mongoDbService.GetObjectById<Category>(category.Id, nameof(Category));
            if (categoryToBeUpdated == null || categoryToBeUpdated.Id != category.Id)
            {
                return UpdateStatus.NotFound;
            }
            bool Filter(Category c) => c.Name == category.Name;
            var checkCategory = _mongoDbService.GetObjectByFilter(nameof(Category), (Func<Category, bool>)Filter);
            if (checkCategory != null && checkCategory.Name == category.Name && checkCategory.Id != category.Id)
            {
                return UpdateStatus.BadRequest;
            }
            _mongoDbService.UpdateObject(nameof(Category), category);
            return UpdateStatus.Success;
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            categories = _mongoDbService.GetList<Category>(nameof(Category));
            return categories;
        }

        public List<Category> GetRootCategories()
        {
            var categories = _mongoDbService.GetList<Category>(nameof(Category));
            var rootCategories = categories.FindAll(category => category.ParentCategoryId == "");
            return rootCategories;
        }

        public List<Category> GetCategoriesByParent(string categoryId)
        {
            var categories = _mongoDbService.GetList<Category>(nameof(Category));
            var filteredCatgories = categories.FindAll(c => c.ParentCategoryId == categoryId);
            return filteredCatgories;
        }
    }
}
