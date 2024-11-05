namespace Contracts.DTO
{
    public class CustomerDTO
    {
        public required string email { get; set; }
        public required string token { get; set; }
        public required string refreshToken { get; set; }
    }
}
