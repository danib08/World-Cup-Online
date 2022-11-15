using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Scorer_In_BetController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly Scorer_In_BetData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Scorer_In_BetController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new Scorer_In_BetData();
        }

        /// <summary>
        /// Servcice to get all Scorer_In_Bet
        /// </summary>
        /// <returns>List of Scorer_In_Bet</returns>
        [HttpGet]
        public async Task<ActionResult<List<Scorer_In_Bet>>> Get()
        {
            return await _funct.GetScorer_In_Bet();
        }

        /// <summary>
        /// Service to get one Scorer_In_Bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Scorer_In_Bet</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Scorer_In_Bet>> GetOne(int id)
        {
            return await _funct.GetOneScorer_In_Bet(id); ;
        }

        /// <summary>
        /// Service to insert Scorer_In_Bet
        /// </summary>
        /// <param name="scorer_In_Bet"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Scorer_In_Bet scorer_In_Bet)
        {
            await _funct.CreateScorer_In_Bet(scorer_In_Bet);
        }

        /// <summary>
        /// Service to delete Scorer_In_Bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteScorer_In_Bet(id);
        }
    }
}
