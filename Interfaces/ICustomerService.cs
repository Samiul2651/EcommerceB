using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.Extensions.Options;

namespace EcommerceWebApi.Interfaces
{
    public interface ICustomerService
    {
        public string LogIn(string email, string password);

        public string Register(Customer customer);

        public bool SubmitOrder(Order order);
    }
}
