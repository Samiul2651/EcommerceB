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

        public async Task<string> AddCategory(Category newCategory)
        {
            bool Filter(Category category) => category.Name == newCategory.Name;
            var checkCategory = await _mongoDbService.GetObjectByFilter(nameof(Category), (Func<Category, bool>)Filter);
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

        public async Task<string> DeleteCategory(Category category)
        {
            
            bool result = await _mongoDbService.DeleteObject<Category>(nameof(Category), category.Id);
            if (!result)
            {
                return UpdateStatus.BadRequest;
            }
            bool Filter(Product product) => product.Category == category.Id;
            var productsByCategory = await _mongoDbService.GetListByFilter(nameof(Product), (Func<Product, bool>)Filter);
            foreach (var product in productsByCategory)
            {
                product.Category = "";
                _mongoDbService.UpdateObject(nameof(Product), product);
            }
            return UpdateStatus.Success;
        }

        public async Task<string> UpdateCategory(Category category)
        {
            var categoryToBeUpdated = await _mongoDbService.GetObjectById<Category>(category.Id, nameof(Category));
            if (categoryToBeUpdated == null || categoryToBeUpdated.Id != category.Id)
            {
                return UpdateStatus.NotFound;
            }
            bool Filter(Category c) => c.Name == category.Name;
            var checkCategory = await _mongoDbService.GetObjectByFilter(nameof(Category), (Func<Category, bool>)Filter);
            if (checkCategory != null && checkCategory.Name == category.Name && checkCategory.Id != category.Id)
            {
                return UpdateStatus.BadRequest;
            }
            _mongoDbService.UpdateObject(nameof(Category), category);
            return UpdateStatus.Success;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = new List<Category>();
            categories = await _mongoDbService.GetList<Category>(nameof(Category));
            return categories;
        }

        public async Task<List<Category>> GetTrendingCategories()
        {
            var categories = await GetAllCategories();
            var sortedCategories = categories.OrderByDescending(c => c.TrendingScore).Take(10).ToList();
            return sortedCategories;
        }

        public async Task<List<Category>> GetRootCategories()
        {
            var categories = await _mongoDbService.GetList<Category>(nameof(Category));
            var rootCategories = categories.FindAll(category => category.ParentCategoryId == "");
            return rootCategories;
        }

        public async Task<List<Category>> GetCategoriesByParent(string categoryId)
        {
            var categories = await _mongoDbService.GetList<Category>(nameof(Category));
            var filteredCatgories = categories.FindAll(c => c.ParentCategoryId == categoryId);
            return filteredCatgories;
        }

        public async Task<List<Category>> GetCategoriesBySearch(string input)
        {
            var categories = await _mongoDbService.GetList<Category>(nameof(Category));
            var filteredCategories = new List<Category>();
            foreach (var category in categories)
            {
                if ((category.Name.ToLower()).Contains(input.ToLower()) && input != "")
                {
                    filteredCategories.Add(category);

                }
                else if (input == "") filteredCategories.Add(category);
            }
            return filteredCategories;
        }

        public async Task<Category> GetCategoryById(string id)
        {
            var category = await _mongoDbService.GetObjectById<Category>(id, nameof(Category));
            return category;
        }

        public async Task AddTrendingScore(string categoryId, int value)
        {
            while (true)
            {
                var category = await GetCategoryById(categoryId);
                if(category == null || category.Id != categoryId)break;
                category.TrendingScore += value;
                _mongoDbService.UpdateObject(nameof(Category), category);
                if (category.ParentCategoryId != "")
                {
                    categoryId = category.ParentCategoryId;
                    continue;
                }
                break;
            }
        }
    }
}
