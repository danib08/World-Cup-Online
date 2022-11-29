using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using System.Numerics;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public PlayerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<Player>>> Get()
        {
            var function = new PlayerData();

            var list = await function.GetPlayers();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Player>>> GetOne(string id)
        {
            var function = new PlayerData();
            var player = new Player();
            player.id = id;
            var list = await function.GetOnePlayer(player);
            return list;
        }


        [HttpPost]
        public async Task Post([FromBody] Player player)
        {
            var function = new PlayerData();
            await function.PostPlayer(player);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Player player)
        {
            var function = new PlayerData();
            player.id = id;
            await function.PutPlayer(player);

        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var function = new PlayerData();
            var player = new Player();
            player.id = id;
            await function.DeletePlayer(player);
        }
    }
}



