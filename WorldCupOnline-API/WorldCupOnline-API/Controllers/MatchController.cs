using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MatchData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new MatchData();
        }

        /// <summary>
        /// Service to get all Matches
        /// </summary>
        /// <returns>List of Match</returns>
        [HttpGet]
        public async Task<ActionResult<List<Match>>> Get()
        {
            return await _funct.GetMatches();
        }

        /// <summary>
        /// Service to get one match
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Match</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetOne(int id)
        {
            return await _funct.GetOneMatch(id);
        }

        /// <summary>
        /// Service to insert Match
        /// </summary>
        /// <param name="match"></param>
        /// <returns>Task action result</returns>
        [HttpPost]
        public async Task Post([FromBody] MatchCreator match)
        {
            await _funct.CreateMatch(match);
        }

        /// <summary>
        /// Service to edit Match
        /// </summary>
        /// <param name="id"></param>
        /// <param name="match"></param>
        /// <returns>Task action result</returns>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Match match)
        {
            await _funct.EditMatch(id, match);
        }

        /// <summary>
        /// Service to delete Match
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task action result</returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteMatch(id);
        }
    }
}
