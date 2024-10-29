using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.Extensions.Options;

namespace EcommerceWebApi.Interfaces
{
    public interface ICustomerService
    {
        public bool LogIn(string email, string password);


        public bool Register(Customer customer);

        public void SubmitOrder(Order order);
    }
}
