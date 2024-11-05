using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class OrderService : IOrderService
    {
        private IMongoDbService _mongoDbService;

        public OrderService(IMongoDbService mongoDbService, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbService = mongoDbService;
        }

        public bool SubmitOrder(Order order)
        {

            Order newOrder = new Order()
            {
                CustomerId = order.CustomerId,
                Products = order.Products,
                Address = order.Address,
                PhoneNumber = order.PhoneNumber,
                Price = order.Price,
                OrderTime = order.OrderTime,
            };
            return _mongoDbService.AddObject(nameof(Order), newOrder);

        }
    }
}
