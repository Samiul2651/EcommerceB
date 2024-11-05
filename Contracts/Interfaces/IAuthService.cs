using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface IAuthService
    {
        public string LogIn(string email, string password);

        public string Register(Customer customer);
    }
}
