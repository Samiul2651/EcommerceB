using Contracts.DTO;

namespace Business.Interfaces
{
    public interface ITokenService
    {
        string GetToken(string email);
        public Task<string> GetRefreshToken(string email);
        public Task<bool> CheckRefreshToken(TokenDTO tokenDto);
    }
}
