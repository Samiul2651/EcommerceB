using MongoDB.Driver;
using Microsoft.Extensions.Options;
using EcommerceWebApi.Models;
using MongoDB.Bson;
using EcommerceWebApi.Interfaces;

namespace EcommerceWebApi.Services
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;


        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        //public IMongoCollection<T> GetCollection<T>(string collectionName)
        //{
        //    return _database.GetCollection<T>(collectionName);
        //}


        public bool IsAlive()
        {
            //_database.GetCollection<Product>(nameof(Product));
            try
            {
                var command = new JsonCommand<BsonDocument>("{ ping: 1 }");
                _database.RunCommand(command);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection error: {ex.Message}");
                return false;
            }
        }

        public T GetObjectById<T>(string id, string collectionName) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            return _collection.Find(p => p.Id == id).FirstOrDefault();
        }

        public bool AddObject<T>(string collectionName, T value) where T : IModel
        {
            try
            {
                IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
                _collection.InsertOne(value);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool UpdateObject<T>(string collectionName, T value) where T : IModel
        {
            try
            {
                IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
                _collection.ReplaceOne(p => p.Id == value.Id, value);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool DeleteObject<T>(string collectionName, string id) where T : IModel
        {
            try
            {
                IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
                _collection.DeleteOne(p => p.Id == id);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public List<T> GetList<T>(string collectionName) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            return _collection.Find(p => true).ToList();
        }



        public bool LogIn(string email, string password)
        {
            IMongoCollection<Customer> _collection = _database.GetCollection<Customer>(nameof(Customer));
            Customer user = _collection.Find(user => user.Email == email).FirstOrDefault();
            //Console.WriteLine(password + " " +  user.Password);
            if (user == null)
            {
                return false;
            }

            if (user.Password != password)
            {
                return false;
            }

            return true;
        }

        public bool Register(Customer customer)
        {
            IMongoCollection<Customer> _collection = _database.GetCollection<Customer>(nameof(Customer));
            Customer user = _collection.Find(user => user.Email == customer.Email).FirstOrDefault();
            if (user != null && user.Email == customer.Email)
            {
                return false;
            }

            _collection.InsertOne(customer);
            return true;
        }

        public bool AddCategoryToProduct(string categoryId, string productId)
        {
            IMongoCollection<Product> _productCollection = _database.GetCollection<Product>(nameof(Product));
            Product product = _productCollection.Find(product => product.Id == productId).FirstOrDefault();
            if (product != null && product.Id == productId)
            {
                product.Category = categoryId;
                DeleteObject<Product>(nameof(Product), productId);
                AddObject(nameof(Product), product);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Category> GetCategoriesByParent(string categoryId)
        {
            IMongoCollection<Category> _categiryCollection = _database.GetCollection<Category>(nameof(Category));
            var categories = _categiryCollection.Find(category => category.ParentCategoryId == categoryId).ToList();
            return categories;
        }

        public List<Product> GetProductsByCategory(string categoryId)
        {
            IMongoCollection<Product> _productCollection = _database.GetCollection<Product>(nameof(Product));
            var products = _productCollection.Find(product => product.Category == categoryId).ToList();
            return products;
        }

        public string GetRefreshToken(string userId)
        {
            IMongoCollection<RefreshToken> _tokenCollection =
                _database.GetCollection<RefreshToken>(nameof(RefreshToken));
            var token = _tokenCollection.Find(token => token.UserId == userId).FirstOrDefault();
            if (token != null && token.UserId == userId)
            {
                return token.Token;
            }

            return null;
        }

        public void UpdateRefreshToken(RefreshToken refreshToken)
        {
            Console.WriteLine(refreshToken.UserId);
            IMongoCollection<RefreshToken> _tokenCollection =
                _database.GetCollection<RefreshToken>(nameof(RefreshToken));
            var token = _tokenCollection.Find(token => token.UserId == refreshToken.UserId).FirstOrDefault();
            //Console.WriteLine(refreshToken.UserId);
            if (token != null && token.UserId == refreshToken.UserId)
            {
                _tokenCollection.ReplaceOne(token => token.UserId == refreshToken.UserId, refreshToken);
            }
            else
            {
                //Console.WriteLine(">>>");
                _tokenCollection.InsertOne(refreshToken);
            }
        }
    }
}