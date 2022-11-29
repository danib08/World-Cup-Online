using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Repositories
{
    [Route("api/League")]
    [ApiController]
    public class LeagueRepository : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LeagueData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public LeagueRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new LeagueData();
        }


        /// <summary>
        /// Service to get all Leagues
        /// </summary>
        /// <returns>List of League</returns>
        [HttpGet]
        public async Task<ActionResult<List<League>>> Get()
        {
            return await _funct.GetLeagues();
        }

        /// <summary>
        /// Service to get one league
        /// </summary>
        /// <param name="id"></param>
        /// <returns>League</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<League>> GetOne(string id)
        {
            return await _funct.GetOneLeague(id);
        }

        /// <summary>
        /// Service to get list of Tournaments for Leagues
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Tournaments")]
        public async Task<ActionResult<List<ValueStringBody>>> GetTournamentsLeagues()
        {
            return await _funct.GetTournaments();
        }

        /// <summary>
        /// Service to insert League
        /// </summary>
        /// <param name="league"></param>
        /// <returns>Task action result</returns>
        [HttpPost]
        public IActionResult Post([FromBody] LeagueCreator league)
        {
            string accessCode = _funct.CreateLeague(league).Result;
            return Ok(accessCode);
        }
    }
}
