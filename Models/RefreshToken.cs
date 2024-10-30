namespace EcommerceWebApi.Models
{
    public class RefreshToken
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
    }
}
