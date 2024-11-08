using Contracts.Models;

namespace Contracts.Interfaces{
    public interface IProductService
    {

        Product? GetProductById(string id);

        List<Product> GetProductsByPage(int page);

        bool AddProduct(Product product);

        string DeleteProduct(string id);

        string UpdateProduct(Product product);

        List<Product> GetProductsBySearchAndPage(string input, int page);
        List<Product> GetProductsBySearchAndPageWithId(string input, int page);
        public List<Category> GetAllCategories();
        public List<Product> GetAllProductsByCategory(string categoryId, int page);
        public List<Category> GetRootCategories();
        public List<Category> GetCategoriesByParent(string categoryId);
        public List<Product> GetProductsByIds(List<string> ids);
    }
}