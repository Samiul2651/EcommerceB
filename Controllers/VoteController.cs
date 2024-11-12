using Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : Controller
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [HttpGet("getVote/{productId}/{userId}")]
        public IActionResult getVote(string productId, string userId)
        {
            var vote = _voteService.GetVote(productId, userId);
            return Ok(vote);
        }
    }
}
