using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Assist_In_BetController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly Assist_In_BetData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Assist_In_BetController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new Assist_In_BetData();
        }

        /// <summary>
        /// Service to get all assist_in_bet
        /// </summary>
        /// <returns>List of Assist_In_Bet</returns>
        [HttpGet]
        public async Task<ActionResult<List<Assist_In_Bet>>> Get()
        {
            return await _funct.GetAssist_In_Bet();
        }

        /// <summary>
        /// Service to get one Assist_In_Bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Assist_In_Bet</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Assist_In_Bet>> GetOne(int id)
        {
            return await _funct.GetOneAssist_In_Bet(id); ;
        }

        /// <summary>
        /// Service to insert an Assist_In_Bet
        /// </summary>
        /// <param name="assist_In_Bet"></param>
        /// <returns>Task action result</returns>
        [HttpPost]
        public async Task Post([FromBody] Assist_In_Bet assist_In_Bet)
        {
            await _funct.CreateAssist_In_Bet(assist_In_Bet);
        }

        /// <summary>
        /// Service to delete Assist_In_Bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task action result</returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteAssist_In_Bet(id);
        }
    }
}
