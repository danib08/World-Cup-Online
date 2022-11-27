using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_In_LeagueController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly User_In_LeagueData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public User_In_LeagueController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new User_In_LeagueData();
        }

        /// <summary>
        /// Service to get all User_In_League
        /// </summary>
        /// <returns>List of League</returns>
        [HttpGet]
        public async Task<ActionResult<List<User_In_League>>> Get()
        {
            return await _funct.GetUIL();
        }

        /// <summary>
        /// Service to get one User_In_League
        /// </summary>
        /// <param name="id"></param>
        /// <returns>League</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User_In_League>> GetOne(int id)
        {
            return await _funct.GetOneUIL(id);
        }

        /// <summary>
        /// Service to insert User_In_League
        /// </summary>
        /// <param name="league"></param>
        /// <returns>Task action result</returns>
        [HttpPost]
        public async Task Post([FromBody] User_In_League uil)
        {
            await _funct.CreateUIL(uil);
        }
         
        /// <summary>
        /// Service to delete User_In_League
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task action result</returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteUIL(id);
        }

    }
}
