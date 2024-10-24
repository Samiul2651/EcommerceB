using EcommerceWebApi.Interfaces;
using MongoDB.Bson;

namespace EcommerceWebApi.Models
{
    public class Customer : IModel
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
