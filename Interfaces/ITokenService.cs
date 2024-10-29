using EcommerceWebApi.Models;

namespace EcommerceWebApi.Interfaces
{
    public interface ITokenService
    {
        string GetToken(Customer customer);
    }
}
