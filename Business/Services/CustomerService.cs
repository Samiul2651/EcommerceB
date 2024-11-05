using Contracts.Constants;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private IMongoDbService _mongoDbService;

        public CustomerService(IMongoDbService mongoDbService, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbService = mongoDbService;
        }

        //public string LogIn(string email, string password)
        //{
        //    try
        //    {
        //        bool result = _mongoDbService.LogIn(email, password);
        //        if (result) return UpdateStatus.Success;
        //        return UpdateStatus.NotFound;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return UpdateStatus.Failed;
        //    }
        //}

        //public string Register(Customer customer)
        //{
        //    Customer newCustomer = new Customer()
        //    {
        //        Name = customer.Name,
        //        Email = customer.Email,
        //        Password = customer.Password
        //    };
        //    try
        //    {
        //        var result = _mongoDbService.Register(newCustomer);
        //        if(result)return UpdateStatus.Success;
        //        return UpdateStatus.BadRequest;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.ToString());
        //        return UpdateStatus.Failed;
        //    }
        //}

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
