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
        private readonly Team_In_MatchData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Team_In_MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new Team_In_MatchData();
        }

        [HttpGet]
        public async Task<ActionResult<List<Team_In_Match>>> Get()
        {
            return await _funct.GetTeam_In_Match();
        }

        [HttpGet("{teamid}/{matchid}")]
        public async Task<ActionResult<Team_In_Match>> GetOne(string teamid, int matchid)
        {
            return await _funct.GetOneTeam_In_Match(teamid, matchid); ;
        }

        [HttpPost]
        public async Task Post([FromBody] Team_In_Match team_In_Match)
        {
            await _funct.CreateTeam_In_Match(team_In_Match);
        }

        [HttpDelete("{teamid}/{matchid}")]
        public async Task Delete(string teamid, int matchid)
        {
            await _funct.DeleteTeam_In_Match(teamid, matchid);
        }
    }
}
