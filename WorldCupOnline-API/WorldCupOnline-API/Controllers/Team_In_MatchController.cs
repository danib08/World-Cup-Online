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
    public class Team_In_MatchController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Team_In_MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Team_In_Match>>> Get()
        {
            var function = new Team_In_MatchData();

            var list = await function.GetTeam_In_Match();
            return list;
        }

        [HttpGet("{teamid}/{matchid}")]
        public async Task<ActionResult<List<Team_In_Match>>> GetOne(string teamid, int matchid)
        {
            var function = new Team_In_MatchData();
            var team_In_Match = new Team_In_Match();
            team_In_Match.teamid = teamid;
            team_In_Match.matchid = matchid;
            var list = await function.GetOneTeam_In_Match(team_In_Match);
            return list;
        }


        [HttpPost]
        public async Task Post([FromBody] Team_In_Match team_In_Match)
        {
            var function = new Team_In_MatchData();
            await function.PostTeam_In_Match(team_In_Match);
        }

        [HttpPut("{teamid}/{matchid}")]
        public async Task Put(string teamid, int matchid, [FromBody] Team_In_Match team_In_Match)
        {
            var function = new Team_In_MatchData();
            team_In_Match.teamid = teamid;
            team_In_Match.matchid = matchid;
            await function.PutTeam_In_Match(team_In_Match);

        }

        [HttpDelete("{teamid}/{matchid}")]
        public async Task Delete(string teamid, int matchid)
        {
            var function = new Team_In_MatchData();
            var team_In_Match = new Team_In_Match();
            team_In_Match.teamid = teamid;
            team_In_Match.matchid = matchid;
            await function.DeleteTeam_In_Match(team_In_Match);
        }
    }
}
