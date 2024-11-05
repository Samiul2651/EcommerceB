using Contracts.DTO;

namespace Business.Interfaces
{
    public interface ITokenService
    {
        string GetToken(string email);
        public string GetRefreshToken(string email);
        public bool CheckRefreshToken(TokenDTO tokenDto);
    }
}
