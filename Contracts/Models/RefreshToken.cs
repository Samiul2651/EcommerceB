using Contracts.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Models
{
    public class RefreshToken : IModel
    {
        [BsonId]
        public required string Id { get; set; }
        public required string Token { get; set; }
    }
}
