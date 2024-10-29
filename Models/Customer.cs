using EcommerceWebApi.Interfaces;
using MongoDB.Bson;

namespace EcommerceWebApi.Models
{
    public class Customer : IModel
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
}
