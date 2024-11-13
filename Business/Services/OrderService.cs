using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Business.Services
{
    public class OrderService : IOrderService
    {
        private IMongoDbService _mongoDbService;

        public OrderService(IMongoDbService mongoDbService, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbService = mongoDbService;
        }

        public async void SubmitOrder(Order order)
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
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "order", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            string message = "Order Placed!" + order.CustomerId;
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "order", body: body);

            Console.WriteLine(message);
        }
    }
}
