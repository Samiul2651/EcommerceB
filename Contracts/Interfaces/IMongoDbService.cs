namespace Contracts.Interfaces
{
    public interface IMongoDbService
    {
        public T GetObjectById<T>(string id, string collectionName) where T : IModel;
        public bool AddObject<T>(string collectionName, T value) where T : IModel;
        public bool UpdateObject<T>(string collectionName, T value) where T : IModel;
        public bool DeleteObject<T>(string collectionName, string id) where T : IModel;
        public List<T> GetList<T>(string collectionName) where T : IModel;
        public List<T> GetListByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel;
        public T GetObjectByFilter<T>(string collectionName, Func<T, bool> filter) where T : IModel;
    }
}
