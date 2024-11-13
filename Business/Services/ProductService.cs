using Contracts.Constants;
using Contracts.DTO;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class ProductService : IProductService {
        
        private IMongoDbService _mongoDbService;
        private readonly ICategoryService _categoryService;

        public ProductService(IMongoDbService mongoDbService, ICategoryService categoryService)
        {
            _mongoDbService = mongoDbService;
            _categoryService=categoryService;
        }


        public async Task<bool> AddProduct(Product product)
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

            bool result = await _mongoDbService.AddObject(nameof(Product), newProduct);
            if(result)return true;
            return false;
        }

        public async Task<string> DeleteProduct(string id)
        {
            try
            {
                var productToBeDeleted = await _mongoDbService.GetObjectById<Product>(id, nameof(Product));
                if (productToBeDeleted == null || productToBeDeleted.Id != id)
                {
                    return UpdateStatus.NotFound;
                }
                
                bool result = await _mongoDbService.DeleteObject<Product>(nameof(Product), id);

                if(result)return UpdateStatus.Success;
                return UpdateStatus.Failed;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return UpdateStatus.Failed;
            }
        }

        public async Task<string> UpdateProduct(Product product)
        {
            try
            {
                var productToBeUpdated = await _mongoDbService.GetObjectById<Product>(product.Id, nameof(Product));
                if (productToBeUpdated == null || productToBeUpdated.Id != product.Id)
                {
                    return UpdateStatus.NotFound;
                }

                bool result = await _mongoDbService.UpdateObject(nameof(Product), product);
                if (result) return UpdateStatus.Success;
                return UpdateStatus.Failed;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return UpdateStatus.Failed;
            }
            
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _mongoDbService.GetObjectById<Product>(id, nameof(Product));
        }

        public async Task<ProductsDTO> GetProductsByPage(int page)
        {
            var products = await _mongoDbService.GetList<Product>(nameof(Product));
            var sortedProducts = products.OrderByDescending(p => p.TrendingScore).ToList();
            var productsByPage = new List<Product>();
            int startingIndex = Math.Min((page - 1) * 10, products.Count);
            int endingIndex = Math.Min(products.Count, page * 10);
            Console.WriteLine(startingIndex + " "  +  endingIndex);
            for (int i = startingIndex; i < endingIndex; i++)
            {
                productsByPage.Add(sortedProducts[i]);
            }
            var productsDto = new ProductsDTO();
            productsDto.products = productsByPage;
            productsDto.maxPage = GetPageCount(products.Count);
            return productsDto;
        }

        public async Task<ProductsDTO> GetProductsBySearchAndPage(string input, int page)
        {
            var products = await _mongoDbService.GetList<Product>(nameof(Product));
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

            var productDto = new ProductsDTO();
            productDto.products = productsByPage;
            productDto.maxPage = GetPageCount(filteredProducts.Count);
            return productDto;
        }

        public async Task<ProductsDTO> GetProductsBySearchAndPageWithId(string input, int page)
        {
            var products = await _mongoDbService.GetList<Product>(nameof(Product));
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
            var productDto = new ProductsDTO();
            productDto.products = productsByPage;
            productDto.maxPage = GetPageCount(filteredProducts.Count);
            return productDto;
        }

        public async Task<ProductsDTO> GetAllProductsByCategory(string categoryId, int page)
        {
            var products = new List<Product>();
            Queue<Category> queue = new Queue<Category>();
            var category = await _mongoDbService.GetObjectById<Category>(categoryId, nameof(Category)); Console.WriteLine(category.Name);
            if(category != null)queue.Enqueue(category);
            while (queue.Count > 0)
            {
                var id = queue.Peek().Id;
                bool Filter(Category category) => category.ParentCategoryId == id;
                var categories = await _mongoDbService.GetListByFilter(nameof(Category), (Func<Category, bool>)Filter);
                categories.ForEach(item =>
                {
                    queue.Enqueue(item);
                });
                bool ProductFilter(Product product) => product.Category == id;
                var productsByCategory = await _mongoDbService.GetListByFilter(nameof(Product), (Func<Product, bool>)ProductFilter);
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
            var productDto = new ProductsDTO();
            productDto.products = productsByPage;
            productDto.maxPage = GetPageCount(products.Count);
            return productDto;
        }

        public async Task<List<Product>> GetProductsByIds(List<string> ids)
        {
            var products = new List<Product>();
            foreach (var produtcId in ids)
            {
                Product product = await GetProductById(produtcId);
                products.Add(product);
            }
            return products;
        }



        public int GetPageCount(int count)
        {
            int maxPage = count / 10;
            if (count % 10 != 0) maxPage += 1;
            return maxPage;
        }

        public async Task UpvoteProduct(string productId, string userId)
        {
            bool Filter(Vote v) => v.UserId == userId && v.ProductId == productId;
            Vote vote = await _mongoDbService.GetObjectByFilter(nameof(Vote), (Func<Vote, bool>)Filter);
            Vote newVote;
            if (vote == null)
            {
                AddTrendingScore(productId, 1);
                newVote = new Vote
                {
                    ProductId = productId,
                    UserId = userId,
                    IsPositive = true
                };
                _mongoDbService.AddObject(nameof(Vote), newVote);
            }
            else if (vote.ProductId == productId && vote.UserId == userId)
            {
                if (!vote.IsPositive)
                {
                    AddTrendingScore(productId, 2);
                    vote.IsPositive = true;
                    _mongoDbService.UpdateObject(nameof(Vote), vote);
                }
            }
            else
            {
                AddTrendingScore(productId, 1);
                newVote = new Vote
                {
                    ProductId = productId,
                    UserId = userId,
                    IsPositive = true
                };
                await _mongoDbService.AddObject(nameof(Vote), newVote);
            }
        }

        public async Task DownvoteProduct(string productId, string userId)
        {
            bool Filter(Vote v) => (v.UserId == userId) && (v.ProductId == productId);
            Vote vote = await _mongoDbService.GetObjectByFilter(nameof(Vote), (Func<Vote, bool>)Filter);
            Vote newVote;
            if (vote == null)
            {
                AddTrendingScore(productId, -1);
                newVote = new Vote
                {
                    ProductId = productId,
                    UserId = userId,
                    IsPositive = false
                };
                _mongoDbService.AddObject(nameof(Vote), newVote);
                return;
            }
            if (vote.ProductId == productId && vote.UserId == userId)
            {
                if (vote.IsPositive)
                {
                    AddTrendingScore(productId, -2);
                    vote.IsPositive = false;
                    _mongoDbService.UpdateObject(nameof(Vote), vote);
                }
                return;
            }
            AddTrendingScore(productId, -1);
            newVote = new Vote
            {
                ProductId = productId,
                UserId = userId,
                IsPositive = false
            };
            _mongoDbService.AddObject(nameof(Vote), newVote);
        }

        public async Task AddTrendingScore(string productId, int value)
        {
            var product = await _mongoDbService.GetObjectById<Product>(productId, nameof(Product));
            product.TrendingScore += value;
            _categoryService.AddTrendingScore(product.Category, value);
            _mongoDbService.UpdateObject(nameof(Product), product);
        }
    }
}