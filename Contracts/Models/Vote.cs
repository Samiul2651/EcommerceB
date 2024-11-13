using Contracts.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Models
{
    public class Vote : IModel
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string ProductId { get; set; }
        public string UserId { get; set; }
        public bool IsPositive { get; set; }
    }
}
