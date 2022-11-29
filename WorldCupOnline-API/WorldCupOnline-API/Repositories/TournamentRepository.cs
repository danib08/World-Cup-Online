using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Repositories
{
    [Route("api/Tournament")]
    [ApiController]
    public class TournamentRepository : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TournamentData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TournamentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new TournamentData();
        }

        /// <summary>
        /// Service to get all tournaments
        /// </summary>
        /// <returns>List of GetTournamentBody</returns>
        [HttpGet]
        public async Task<ActionResult<List<GetTournamentBody>>> Get()
        {
            return await _funct.GetTournament();
        }

        /// <summary>
        /// Service to get one tournament 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>GetTournamentBody</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTournamentBody>> GetOne(string id)
        {
            return await _funct.GetOneTournament(id);
        }

        /// <summary>
        /// Service to insert tournament
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] TournamentCreator tournament)
        {
            await _funct.CreateTournament(tournament);
        }

        /// <summary>
        /// Service to get all matches in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of MatchTournamentBody</returns>
        [HttpGet("Matches/{id}")]
        public async Task<ActionResult<List<MatchTournamentBody>>> GetMatchesTournament(string id)
        {
            return await _funct.GetMatchesByTournament(id);
        }

        /// <summary>
        /// Service to get all phases in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of ValueIntBody</returns>
        [HttpGet("Phases/{id}")]
        public async Task<ActionResult<List<ValueIntBody>>> GetPhasesByTournament(string id)
        {
            return await _funct.GetPhasesByTournament(id);
        }

        /// <summary>
        /// Service to get all teams in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of TeamTournamentBody</returns>
        [HttpGet("Teams/{id}")]
        public async Task<ActionResult<List<TeamTournamentBody>>> GetTeamsByTournament(string id)
        {
            return await _funct.GetTeamsByTournament(id);
        }

        /// <summary>
        /// Service to get all Types
        /// </summary>
        /// <returns>List of ValueIntBody</returns>
        [HttpGet("Types")]
        public async Task<ActionResult<List<ValueIntBody>>> GetTypes()
        {
            return await _funct.GetTypes();
        }

    }
}
