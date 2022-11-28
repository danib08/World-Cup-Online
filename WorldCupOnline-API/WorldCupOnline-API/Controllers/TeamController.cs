using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private TeamData _funct; 

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TeamController(IConfiguration configuration)
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
        /// Service to insert Team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Team team)
        {
            await _funct.CreateTeam(team);
        }

        /// <summary>
        /// Service to edit Team
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Team team)
        {
            await _funct.EditTeam(id, team);
            
        }

        /// <summary>
        /// Service to delete Team
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeleteTeam(id);  
        }
    }
}


