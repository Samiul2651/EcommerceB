using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface ICustomerService
    {
        //public string LogIn(string email, string password);

        //public string Register(Customer customer);

        public bool SubmitOrder(Order order);
    }
}
