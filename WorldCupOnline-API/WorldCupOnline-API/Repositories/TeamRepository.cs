using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Repositories
{
    [Route("api/Team")]
    [ApiController]
    public class TeamRepository : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TeamData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TeamRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new TeamData();
        }

        /// <summary>
        /// Service to get all Teams
        /// </summary>
        /// <returns>List of IdStringBody</returns>
        [HttpGet]
        public async Task<ActionResult<List<IdStringBody>>> Get()
        {
            return await _funct.GetTeams();
        }

        /// <summary>
        /// Service to get one Team
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Team</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetOne(string id)
        {
            return await _funct.GetOneTeam(id);
        }

        /// <summary>
        /// Service to get all teams of a type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of IdStringBody</returns>
        [HttpGet("Type/{type}")]
        public async Task<ActionResult<List<IdStringBody>>> GetTeamsByType(int type)
        {
            return await _funct.GetTeamsByType(type);
        }

        /// <summary>
        /// Service to get all players of a team
        /// </summary
        /// <param name="teamId"></param>
        /// <returns>List of IdStringBody</returns>
        [HttpGet("{teamId}/Players")]
        public async Task<ActionResult<List<IdStringBody>>> GetPlayersByTeam(string teamId)
        {
            return await _funct.GetPlayersByTeam(teamId);
        }

        /// <summary>
        /// Service to get all Team_In_Tournament
        /// </summary>
        /// <returns>List of Team_In_Tournament</returns>
        [HttpGet("TeamInTournament")]
        public async Task<ActionResult<List<Team_In_Tournament>>> GetTeamsInTournament()
        {
            return await _funct.GetTeam_In_Tournament();
        }
    }
}
