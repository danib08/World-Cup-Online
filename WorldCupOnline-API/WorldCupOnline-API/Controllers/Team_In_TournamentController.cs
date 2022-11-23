using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Team_In_TournamentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Team_In_TournamentData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Team_In_TournamentController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new Team_In_TournamentData();
        }

        /// <summary>
        /// Service to get all Team_In_Tournament
        /// </summary>
        /// <returns>List of Team_In_Tournament</returns>
        [HttpGet]
        public async Task<ActionResult<List<Team_In_Tournament>>> Get()
        {
            return await _funct.GetTeam_In_Tournament();
        }

        /// <summary>
        /// Service to get one Team_In_Tournament
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="tournamentid"></param>
        /// <returns>Team_In_Tournament</returns>
        [HttpGet("{teamid}/{tournamentid}")]
        public async Task<ActionResult<Team_In_Tournament>> GetOne(string teamid, int tournamentid)
        {
            return await _funct.GetOneTeam_In_Tournament(teamid, tournamentid);
        }

        /// <summary>
        /// Service to insert Team_In_Tournament
        /// </summary>
        /// <param name="team_In_Tournament"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Team_In_Tournament team_In_Tournament)
        {
            await _funct.CreateTeam_In_Tournament(team_In_Tournament);
        }

        /// <summary>
        /// Service to delete Team_In_Tournament
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="tournamentid"></param>
        /// <returns></returns>
        [HttpDelete("{teamid}/{tournamentid}")]
        public async Task Delete(string teamid, int tournamentid)
        {
 
            await _funct.DeleteTeam_In_Tournament(teamid, tournamentid);
        }
    }
}


