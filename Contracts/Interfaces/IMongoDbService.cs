namespace Contracts.Interfaces
{
    public interface IMongoDbService
    {
        public Task<T> GetObjectById<T>(string id, string collectionName) where T : IModel;
        public Task<bool> AddObject<T>(string collectionName, T value) where T : IModel;
        public Task<bool> UpdateObject<T>(string collectionName, T value) where T : IModel;
        public Task<bool> DeleteObject<T>(string collectionName, string id) where T : IModel;
        public Task<List<T>> GetList<T>(string collectionName) where T : IModel;
        public Task<List<T>> GetListByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel;
        public Task<T> GetObjectByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel;
    }
}
