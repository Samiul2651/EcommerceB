using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface IAuthService
    {
        public Task<string> LogIn(string email, string password);

        public Task<string> Register(Customer customer);
    }
}
