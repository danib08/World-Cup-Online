using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LeagueData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public LeagueController(IConfiguration configuration)
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
            return await _funct.GetLeague();
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
        /// Service to get one Tournament 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>GetTournamentBody</returns>
        [HttpGet("Tournaments")]
        public async Task<ActionResult<List<ValueIntBody>>> GetLeagues()
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

        /// <summary>
        /// Service to edit League
        /// </summary>
        /// <param name="id"></param>
        /// <param name="league"></param>
        /// <returns>Task action result</returns>
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] League league)
        {
            await _funct.EditLeague(id, league);
        }

        /// <summary>
        /// Service to delete League
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task action result</returns>
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeleteLeague(id);
        }
    }
}
