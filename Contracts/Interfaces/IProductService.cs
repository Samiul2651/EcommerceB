using Contracts.DTO;
using Contracts.Models;

namespace Contracts.Interfaces{
    public interface IProductService
    {

        Task<Product> GetProductById(string id);

        Task<ProductsDTO> GetProductsByPage(int page);

        Task<bool> AddProduct(Product product);

        Task<string> DeleteProduct(string id);

        Task<string> UpdateProduct(Product product);

        Task<ProductsDTO> GetProductsBySearchAndPage(string input, int page);
        Task<ProductsDTO> GetProductsBySearchAndPageWithId(string input, int page);
        public Task<ProductsDTO> GetAllProductsByCategory(string categoryId, int page);
        public Task<List<Product>> GetProductsByIds(List<string> ids);
        public Task UpvoteProduct(string productId, string userId);
        public Task DownvoteProduct(string productId, string userId);
        public Task AddTrendingScore(string productId, int value);
    }
}