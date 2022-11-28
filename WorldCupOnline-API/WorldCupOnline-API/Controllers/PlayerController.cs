using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PlayerData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public PlayerController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new PlayerData();
        }

        /// <summary>
        /// Service to get all Player
        /// </summary>
        /// <returns>List of Player</returns>
        [HttpGet]
        public async Task<ActionResult<List<Player>>> Get()
        {
            return await _funct.GetPlayers();
        }

        /// <summary>
        /// Service to get one Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Player</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetOne(string id)
        {
            return await _funct.GetOnePlayer(id);
        }

        /// <summary>
        /// Service to insert Player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Player player)
        {
            await _funct.CreatePlayer(player);
        }

        /// <summary>
        /// Service to edit Player
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Player player)
        {
            await _funct.EditPlayer(id, player);
        }

        /// <summary>
        /// Service to delete Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeletePlayer(id);
        }
    }
}