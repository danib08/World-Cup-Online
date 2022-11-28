using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PhaseData _funct; 

        /// <summary>
        /// Establishconfiguration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public PhaseController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new PhaseData();
        }

        /// <summary>
        /// Service to get all Phases
        /// </summary>
        /// <returns>List of Phase</returns>
        [HttpGet]
        public async Task<ActionResult<List<Phase>>> Get()
        {
            return await _funct.GetPhases(); ;
        }

        /// <summary>
        /// Service to get one Phase
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Phase</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Phase>> GetOne(int id)
        {
            return await _funct.GetOnePhase(id);
        }

        /// <summary>
        /// Service to insert Phase
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Phase phase)
        {
            await _funct.CreatePhase(phase);
        }

        /// <summary>
        /// Service to edit Phase
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phase"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Phase phase)
        {
            await _funct.EditPhase(id, phase);
        }

        /// <summary>
        /// Service to delete Phase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeletePhase(id);  
        }
    }
}

