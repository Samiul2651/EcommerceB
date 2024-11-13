using Contracts.Interfaces;
using Contracts.Models;

namespace Business.Services
{
    public class VoteService : IVoteService
    {
        private readonly IMongoDbService _mongoDbService;

        public VoteService(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<Vote> GetVote(string productId, string userId)
        {
            bool Filter(Vote v) => v.UserId == userId && v.ProductId == productId;
            Vote vote = await _mongoDbService.GetObjectByFilter(nameof(Vote), (Func<Vote, bool>)Filter);
            if (vote == null)
            {
                return null;
            }
            if (vote.ProductId != productId || vote.UserId != userId)
            {
                return null;
            }
            return vote;
        }
    }
}
