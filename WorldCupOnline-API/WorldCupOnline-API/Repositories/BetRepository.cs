using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Interfaces;

namespace WorldCupOnline_API.Repositories
{
    [Route("api/Bet")]
    [ApiController]
    public class BetRepository : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBetData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public BetRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new BetData();
        }

        /// <summary>
        /// Service to get all Bet
        /// </summary>
        /// <returns>List of Bet</returns>
        [HttpGet]
        public async Task<ActionResult<List<Bet>>> Get()
        {
            return await _funct.GetBets();
        }

        /// <summary>
        /// Service to get one Bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bet</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Bet>> GetOne(int id)
        {
            return await _funct.GetOneBet(id);
        }

        /// <summary>
        /// Service to create a Bet
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="userId"></param>
        /// <param name="matchId"></param>
        /// <returns>Task Action result</returns>
        [HttpPost("{userId}/{matchId}")]
        public async Task Post([FromBody] BetCreator bet, string userId, int matchId)
        {
            await _funct.CreateBet(userId, matchId, bet);
        }
    }
}
