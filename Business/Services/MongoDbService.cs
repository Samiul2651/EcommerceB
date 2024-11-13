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

        public async Task<T> GetObjectById<T>(string id, string collectionName) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> AddObject<T>(string collectionName, T value) where T : IModel
        {
            try
            {
                IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
                await _collection.InsertOneAsync(value);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateObject<T>(string collectionName, T value) where T : IModel
        {
            try
            {
                IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
                await _collection.ReplaceOneAsync(p => p.Id == value.Id, value);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteObject<T>(string collectionName, string id) where T : IModel
        {
            try
            {
                IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
                await _collection.DeleteOneAsync(p => p.Id == id);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async Task<List<T>> GetList<T>(string collectionName) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            return await _collection.Find(p => true).ToListAsync();
        }

        public async Task<List<T>> GetListByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            var list = await _collection.Find(p => true).ToListAsync();
            return list.Where(filter).ToList();
        }

        public async Task<T> GetObjectByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel
        {
            IMongoCollection<T> _collection = _database.GetCollection<T>(collectionName);
            var list = await _collection.Find(p => true).ToListAsync();
            var obj = list.FirstOrDefault(filter);
            return obj;
        }

    }
}