using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly BetData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public BetController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new BetData();
        }

        [HttpGet]
        public async Task<ActionResult<List<Bet>>> Get()
        {
            return await _funct.GetBets();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bet>> GetOne(int id)
        {
            return await _funct.GetOneBet(id);
        }

        [HttpPost("{userId}/{matchId}")]
        public async Task Post([FromBody] BetCreator bet, string userId, int matchId)
        {
            await _funct.CreateBet(userId, matchId, bet);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Bet bet)
        {
            await _funct.EditBet(id, bet);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteBet(id);
        }
    }
}
