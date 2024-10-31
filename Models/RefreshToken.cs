using MongoDB.Bson.Serialization.Attributes;

namespace EcommerceWebApi.Models
{
    public class RefreshToken
    {
        [BsonId]
        public required string UserId { get; set; }
        public required string Token { get; set; }
    }
}
