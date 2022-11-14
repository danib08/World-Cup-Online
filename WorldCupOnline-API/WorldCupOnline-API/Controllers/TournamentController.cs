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

        [HttpGet]
        public async Task<ActionResult<List<GetTournamentBody>>> Get()
        {
            return await _funct.GetTournament();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTournamentBody>> GetOne(int id)
        {
            return await _funct.GetOneTournament(id);
        }

        [HttpPost]
        public async Task Post([FromBody] TournamentCreator tournament)
        {
            await _funct.CreateTournament(tournament);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Tournament tournament)
        {
            await _funct.EditTournament(id, tournament);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteTournament(id);
        }

        [HttpGet("Matches/{id}")]
        public async Task<ActionResult<List<MatchTournamentBody>>> GetMatchesTournament(int id)
        {
            return await _funct.GetMatchesByTournament(id);
        }

        [HttpGet("Phases/{id}")]
        public async Task<ActionResult<List<ValueIntBody>>> GetPhasesByTournament(int id)
        {
            return await _funct.GetPhasesByTournament(id);
        }

        [HttpGet("Teams/{id}")]
        public async Task<ActionResult<List<TeamTournamentBody>>> GetTeamsByTournament(int id)
        {
            return await _funct.GetTeamsByTournament(id);
        }
    }
}