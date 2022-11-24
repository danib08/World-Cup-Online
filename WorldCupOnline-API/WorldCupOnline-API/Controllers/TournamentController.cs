using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TournamentData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TournamentController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new TournamentData();
        }

        /// <summary>
        /// Service to Get all Tournament
        /// </summary>
        /// <returns>List of GetTournamentBody</returns>
        [HttpGet]
        public async Task<ActionResult<List<GetTournamentBody>>> Get()
        {
            return await _funct.GetTournament();
        }


        /// <summary>
        /// Service to get one Tournament 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>GetTournamentBody</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTournamentBody>> GetOne(int id)
        {
            return await _funct.GetOneTournament(id);
        }

        /// <summary>
        /// Service to insert Tournament
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] TournamentCreator tournament)
        {
            await _funct.CreateTournament(tournament);
        }

        /// <summary>
        /// Service to edit Tournament
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tournament"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Tournament tournament)
        {
            await _funct.EditTournament(id, tournament);
        }

        /// <summary>
        /// Service to delete Tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteTournament(id);
        }

        /// <summary>
        /// Service to get all matches in a Tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of MatchTournamentBody</returns>
        [HttpGet("Matches/{id}")]
        public async Task<ActionResult<List<MatchTournamentBody>>> GetMatchesTournament(int id)
        {
            return await _funct.GetMatchesByTournament(id);
        }

        /// <summary>
        /// Service to get all phases in a Tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of ValueIntBody</returns>
        [HttpGet("Phases/{id}")]
        public async Task<ActionResult<List<ValueIntBody>>> GetPhasesByTournament(int id)
        {
            return await _funct.GetPhasesByTournament(id);
        }

        /// <summary>
        /// Service to get teams in a Tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of TeamTournamentBody</returns>
        [HttpGet("Teams/{id}")]
        public async Task<ActionResult<List<TeamTournamentBody>>> GetTeamsByTournament(int id)
        {
            return await _funct.GetTeamsByTournament(id);
        }
    }
}