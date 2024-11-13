using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface IVoteService
    {
        public Task<Vote> GetVote(string productId, string userId);
    }
}
