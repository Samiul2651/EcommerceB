using EcommerceWebApi.Models;

namespace EcommerceWebApi.Interfaces{
    public interface IProductService
    {

        //List<Product> ReadProductsFromJson(string filepath);

        //bool IsAlive();

        //bool WriteProductsToJson(List<Product> products, string filePath);

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
    }
}