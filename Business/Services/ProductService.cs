using Contracts.Constants;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class ProductService : IProductService {
        
        private IMongoDbService _mongoDbService;

        public ProductService(IMongoDbService mongoDbService, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbService = mongoDbService;
        }


        public bool AddProduct(Product product)
        {
            Product newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                TrendingScore = product.TrendingScore,
                Type = product.Type,
                ImageLink = product.ImageLink,
                Category = product.Category
            };

            bool result = _mongoDbService.AddObject(nameof(Product), newProduct);
            if(result)return true;
            return false;
        }

        public string DeleteProduct(string id)
        {
            try
            {
                var productToBeDeleted = _mongoDbService.GetObjectById<Product>(id, nameof(Product));
                if (productToBeDeleted == null || productToBeDeleted.Id != id)
                {
                    return UpdateStatus.NotFound;
                }
                
                bool result = _mongoDbService.DeleteObject<Product>(nameof(Product), id);

                if(result)return UpdateStatus.Success;
                return UpdateStatus.Failed;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return UpdateStatus.Failed;
            }
        }

        public string UpdateProduct(Product product)
        {
            try
            {
                var productToBeUpdated = _mongoDbService.GetObjectById<Product>(product.Id, nameof(Product));
                if (productToBeUpdated == null || productToBeUpdated.Id != product.Id)
                {
                    return UpdateStatus.NotFound;
                }

                bool result = _mongoDbService.UpdateObject(nameof(Product), product);
                if (result) return UpdateStatus.Success;
                return UpdateStatus.Failed;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return UpdateStatus.Failed;
            }
            
        }

        public Product GetProductById(string id)
        {
            return _mongoDbService.GetObjectById<Product>(id, nameof(Product));
        }

        public List<Product> GetProductsByPage(int page)
        {
            var products = _mongoDbService.GetList<Product>(nameof(Product));
            var sortedProducts = products.OrderByDescending(p => p.TrendingScore).ToList();
            var productsByPage = new List<Product>();
            int startingIndex = Math.Min((page - 1) * 10, products.Count);
            int endingIndex = Math.Min(products.Count, page * 10);
            Console.WriteLine(startingIndex + " "  +  endingIndex);
            for (int i = startingIndex; i < endingIndex; i++)
            {
                productsByPage.Add(sortedProducts[i]);
            }
            return productsByPage;
        }

        public List<Product> GetProductsBySearchAndPage(string input, int page)
        {
            var products = _mongoDbService.GetList<Product>(nameof(Product));
            var filteredProducts = new List<Product>();
            foreach (var product in products)
            {
                if ((product.Name).ToLower().Contains(input.ToLower()) && input != "")
                {
                    filteredProducts.Add(product);
                }
                else if(input == "")filteredProducts.Add(product);
            }
            var sortedProducts = filteredProducts.OrderByDescending(p => p.TrendingScore).ToList();
            var productsByPage = new List<Product>();
            int startingIndex = Math.Min((page - 1) * 10, sortedProducts.Count);
            int endingIndex = Math.Min(sortedProducts.Count, page * 10);
            for (int i = startingIndex; i < endingIndex; i++)
            {
                productsByPage.Add(sortedProducts[i]);
            }
            return productsByPage;
        }

        public List<Product> GetProductsBySearchAndPageWithId(string input, int page)
        {
            var products = _mongoDbService.GetList<Product>(nameof(Product));
            var filteredProducts = new List<Product>();
            foreach (var product in products)
            {
                if ((product.Id.ToLower()).Contains(input.ToLower()) && input != "")
                {
                    filteredProducts.Add(product);
                    
                }
                else if (input == "") filteredProducts.Add(product);
            }
            var sortedProducts = filteredProducts.OrderByDescending(p => p.TrendingScore).ToList();
            var productsByPage = new List<Product>();
            int startingIndex = Math.Min((page - 1) * 10, sortedProducts.Count);
            int endingIndex = Math.Min(sortedProducts.Count, page * 10);
            for (int i = startingIndex; i < endingIndex; i++)
            {
                productsByPage.Add(sortedProducts[i]);
            }
            return productsByPage;
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            categories = _mongoDbService.GetList<Category>(nameof(Category));
            return categories;
        }

        public List<Product> GetAllProductsByCategory(string categoryId, int page)
        {
            var products = new List<Product>();
            Queue<Category> queue = new Queue<Category>();
            var category = _mongoDbService.GetObjectById<Category>(categoryId, nameof(Category)); Console.WriteLine(category.Name);
            if(category != null)queue.Enqueue(category);
            while (queue.Count > 0)
            {
                var id = queue.Peek().Id;
                bool Filter(Category category) => category.ParentCategoryId == id;
                var categories = _mongoDbService.GetListByFilter(nameof(Category), (Func<Category, bool>)Filter);
                categories.ForEach(item =>
                {
                    queue.Enqueue(item);
                });
                bool ProductFilter(Product product) => product.Category == id;
                var productsByCategory = _mongoDbService.GetListByFilter(nameof(Product), (Func<Product, bool>)ProductFilter);
                products.AddRange(productsByCategory);
                queue.Dequeue();
            }
            var sortedProducts = products.OrderByDescending(p => p.TrendingScore).ToList();
            var productsByPage = new List<Product>();
            int startingIndex = Math.Min((page - 1) * 10, sortedProducts.Count);
            int endingIndex = Math.Min(sortedProducts.Count, page * 10);
            for (int i = startingIndex; i < endingIndex; i++)
            {
                productsByPage.Add(sortedProducts[i]);
            }
            return productsByPage;
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

        public List<Product> GetProductsByIds(List<string> ids)
        {
            var products = new List<Product>();
            foreach (var produtcId in ids)
            {
                Product product = GetProductById(produtcId);
                products.Add(product);
            }
            return products;
        }



    }
}