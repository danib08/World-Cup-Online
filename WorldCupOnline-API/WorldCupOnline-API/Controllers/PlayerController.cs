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

        [HttpGet]
        public async Task<ActionResult<List<Player>>> Get()
        {
            return await _funct.GetPlayers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Player>>> GetOne(string id)
        {
            return await _funct.GetOnePlayer(id);
        }

        [HttpPost]
        public async Task Post([FromBody] Player player)
        {
            await _funct.CreatePlayer(player);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Player player)
        {
            await _funct.EditPlayer(id, player);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeletePlayer(id);
        }
    }
}