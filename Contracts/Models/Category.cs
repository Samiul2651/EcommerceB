using Contracts.Interfaces;
using MongoDB.Bson;

namespace Contracts.Models
{
    public class Category : IModel
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
        public string ParentCategoryId { get; set; }
    }
}
