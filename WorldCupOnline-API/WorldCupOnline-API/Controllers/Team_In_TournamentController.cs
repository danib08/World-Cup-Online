using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Team_In_TournamentController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Team_In_TournamentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Team_In_Tournament>>> Get()
        {
            var function = new Team_In_TournamentData();

            var list = await function.GetTeam_In_Tournament();
            return list;
        }

        [HttpGet("{teamid}/{tournamentid}")]
        public async Task<ActionResult<List<Team_In_Tournament>>> GetOne(string teamid, int tournamentid)
        {
            var function = new Team_In_TournamentData();
            var team_In_Tournament = new Team_In_Tournament();
            team_In_Tournament.tournamentid = tournamentid;
            team_In_Tournament.teamid = teamid;
            var list = await function.GetOneTeam_In_Tournament(team_In_Tournament);
            return list;
        }


        [HttpPost]
        public async Task Post([FromBody] Team_In_Tournament team_In_Tournament)
        {
            var function = new Team_In_TournamentData();
            await function.PostTeam_In_Tournament(team_In_Tournament);
        }

        [HttpDelete("{teamid}/{tournamentid}")]
        public async Task Delete(string teamid, int tournamentid)
        {
            var function = new Team_In_TournamentData();
            var team_In_Tournament = new Team_In_Tournament();
            team_In_Tournament.teamid = teamid;
            team_In_Tournament.tournamentid = tournamentid;
            await function.DeleteTeam_In_Tournament(team_In_Tournament);
        }
    }
}


