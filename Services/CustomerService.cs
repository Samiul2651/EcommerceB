using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using Microsoft.Extensions.Options;

namespace EcommerceWebApi.Services
{
    public class CustomerService : ICustomerService
    {
        private IMongoDbService _mongoDbService;

        public CustomerService(IMongoDbService mongoDbService, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbService = mongoDbService;
        }

        public bool LogIn(string email, string password)
        {
            return _mongoDbService.LogIn(email, password);
        }

        public bool Register(Customer customer)
        {
            Customer newCustomer = new Customer()
            {
                Name = customer.Name,
                Email = customer.Email,
                Password = customer.Password
            };
            return _mongoDbService.Register(newCustomer);
        }

        public void SubmitOrder(Order order)
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
            _mongoDbService.AddObject(nameof(Order), newOrder);
        }
    }
}
