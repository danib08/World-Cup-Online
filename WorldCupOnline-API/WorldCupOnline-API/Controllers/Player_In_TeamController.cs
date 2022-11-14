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
    public class Player_In_TeamController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Player_In_TeamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<ActionResult<List<Player_In_Team>>> Get()
        {
            var function = new Player_In_TeamData();

            var list = await function.GetPlayer_In_Team();
            return list;
        }

        [HttpGet("{teamid}/{playerid}")]
        public async Task<ActionResult<List<Player_In_Team>>> GetOne(string teamid, string playerid)
        {
            var function = new Player_In_TeamData();
            var player_In_Team = new Player_In_Team();
            player_In_Team.teamid = teamid;
            player_In_Team.playerid = playerid;
            var list = await function.GetOnePlayer_In_Team(player_In_Team);
            return list;
        }

        [HttpPost]
        public async Task Post([FromBody] Player_In_Team player_In_Team)
        {
            var function = new Player_In_TeamData();
            await function.PostPlayer_In_Team(player_In_Team);
        }

        [HttpPut("{teamid}/{playerid}")]
        public async Task Put(string teamid, string playerid, [FromBody] Player_In_Team player_In_Team)
        {
            var function = new Player_In_TeamData();
            player_In_Team.teamid = teamid;
            player_In_Team.playerid = playerid;
            await function.PutPlayer_In_Team(player_In_Team);
            
        }

        [HttpDelete("{teamid}/{playerid}")]
        public async Task Delete(string teamid, string playerid)
        {
            var function = new Player_In_TeamData();
            var player_In_Team = new Player_In_Team();
            player_In_Team.teamid = teamid;
            player_In_Team.playerid = playerid;
            await function.DeletePlayer_In_Team(player_In_Team);  
        }

     }
}
