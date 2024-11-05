using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Contracts.Models;
using MongoDB.Bson;
using Contracts.Interfaces;

namespace Business.Services
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;


        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
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

        public List<T> GetListByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            var list = _collection.Find(p => true).ToList();
            return list.Where(filter).ToList();
        }

        public T GetObjectByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            var list = _collection.Find(p => true).ToList();
            var obj = list.FirstOrDefault(filter);
            return obj;
        }

    }
}