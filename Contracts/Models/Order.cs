using Contracts.Interfaces;
using MongoDB.Bson;

namespace Contracts.Models
{
    public class Order : IModel
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string CustomerId { get; set; }
        public List<Product> Products { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
