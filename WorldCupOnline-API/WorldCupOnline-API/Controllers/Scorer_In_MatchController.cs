using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Scorer_In_MatchController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly Scorer_In_MatchData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Scorer_In_MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new Scorer_In_MatchData();
        }

        /// <summary>
        /// Servcice to get all Scorer_In_Match
        /// </summary>
        /// <returns>List of Scorer_In_Match</returns>
        [HttpGet]
        public async Task<ActionResult<List<Scorer_In_Match>>> Get()
        {
            return await _funct.GetScorer_In_Match();
        }

        /// <summary>
        /// Service to get one Scorer_In_Match
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Scorer_In_Match</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Scorer_In_Match>> GetOne(int id)
        {
            return await _funct.GetOneScorer_In_Match(id); ;
        }

        /// <summary>
        /// Service to insert Scorer_In_Match
        /// </summary>
        /// <param name="scorer_In_Match"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Scorer_In_Match scorer_In_Match)
        {
            await _funct.CreateScorer_In_Match(scorer_In_Match);
        }

        /// <summary>
        /// Service to delete Scorer_In_Match
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteScorer_In_Match(id);
        }
    }
}
