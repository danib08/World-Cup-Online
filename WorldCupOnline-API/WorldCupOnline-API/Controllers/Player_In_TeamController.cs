using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using System.Collections.Generic;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Player_In_TeamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Player_In_TeamData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Player_In_TeamController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new Player_In_TeamData();
        }

        /// <summary>
        /// Service to get all Player_In_Team
        /// </summary>
        /// <returns>List  of Player_In_Team</returns>
        [HttpGet]
        public async Task<ActionResult<List<Player_In_Team>>> Get()
        {
            return await _funct.GetPlayer_In_Team();
        }

        /// <summary>
        /// Service to get one Player_In_Team
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="playerid"></param>
        /// <returns>Player_In_Team</returns>
        [HttpGet("{teamid}/{playerid}")]
        public async Task<ActionResult<Player_In_Team>> GetOne(string teamid, string playerid)
        {
            return await _funct.GetOnePlayer_In_Team(teamid, playerid);
        }

        /// <summary>
        /// Service to insert Player_In_Team
        /// </summary>
        /// <param name="player_In_Team"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Player_In_Team player_In_Team)
        {
            await _funct.CreatePlayer_In_Team(player_In_Team);
        }

        /// <summary>
        /// Service to edit Player_In_Team
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="playerid"></param>
        /// <param name="player_In_Team"></param>
        /// <returns></returns>
        [HttpPut("{teamid}/{playerid}")]
        public async Task Put(string teamid, string playerid, [FromBody] Player_In_Team player_In_Team)
        {
            await _funct.EditPlayer_In_Team(teamid, playerid, player_In_Team);
        }

        /// <summary>
        /// Service to delete Player_In_Team
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="playerid"></param>
        /// <returns></returns>
        [HttpDelete("{teamid}/{playerid}")]
        public async Task Delete(string teamid, string playerid)
        {
            await _funct.DeletePlayer_In_Team(teamid, playerid);  
        }
     }
}
