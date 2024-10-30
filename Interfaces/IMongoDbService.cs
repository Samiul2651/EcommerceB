using EcommerceWebApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EcommerceWebApi.Interfaces
{
    public interface IMongoDbService
    {
        //public IMongoCollection<T> GetCollection<T>(string collectionName);
        public bool IsAlive();

        public T GetObjectById<T>(string id, string collectionName) where T : IModel;

        public bool AddObject<T>(string collectionName, T value) where T : IModel;
        public bool UpdateObject<T>(string collectionName, T value) where T : IModel;
        public bool DeleteObject<T>(string collectionName, string id) where T : IModel;
        public List<T> GetList<T>(string collectionName) where T : IModel;

        public bool LogIn(string email, string password);
        public bool Register(Customer customer);
        public bool AddCategoryToProduct(string categoryId, string productId);
        public List<Category> GetCategoriesByParent(string categoryId);
        public List<Product> GetProductsByCategory(string categoryId);
        public string GetRefreshToken(string userId);
        public void UpdateRefreshToken(RefreshToken refreshToken);
    }
}
