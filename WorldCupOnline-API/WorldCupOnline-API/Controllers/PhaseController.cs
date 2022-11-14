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

        [HttpGet]
        public async Task<ActionResult<List<Phase>>> Get()
        {
            return await _funct.GetPhases(); ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Phase>> GetOne(int id)
        {
            return await _funct.GetOnePhase(id);
        }

        [HttpPost]
        public async Task Post([FromBody] Phase phase)
        {
            await _funct.CreatePhase(phase);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Phase phase)
        {
            await _funct.EditPhase(id, phase);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeletePhase(id);  
        }
    }
}

