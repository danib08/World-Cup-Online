using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Newtonsoft.Json.Linq;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using System.Reflection.Metadata.Ecma335;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TeamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<ActionResult<List<Team>>> Get()
        {
            var function = new TeamData();

            var list = await function.GetTeams();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Team>>> GetOne(string id)
        {
            var function = new TeamData();
            var team = new Team();
            team.id = id;
            var list = await function.GetOneTeam(team);
            return list;
        }

        [HttpGet("Type/{type}")]
        public async Task<ActionResult<List<TeamTypeBody>>> GetType(int type)
        {
            var function = new TeamData();
            var team = new Team();
            team.typeid = type;
            var list = await function.GetType(team);
            return list;
        }


        [HttpPost]
        public async Task Post([FromBody] Team team)
        {
            var function = new TeamData();
            await function.PostTeams(team);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Team team)
        {
            var function = new TeamData();
            team.id = id;
            await function.PutTeam(team);
            
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var function = new TeamData();
            var team = new Team();
            team.id = id;
            await function.DeleteTeam(team);  
        }
    }
}


