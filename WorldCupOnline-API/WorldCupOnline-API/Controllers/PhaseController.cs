using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Newtonsoft.Json.Linq;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public PhaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    [HttpGet]
        public async Task<ActionResult<List<Phase>>> Get()
        {
            var function = new PhaseData();

            var list = await function.GetPhases();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Phase>>> GetOne(int id)
        {
            var function = new PhaseData();
            var phase = new Phase();
            phase.id = id;
            var list = await function.GetOnePhase(phase);
            return list;
        }

        [HttpPost]
        public async Task Post([FromBody] Phase phase)
        {
            var function = new PhaseData();
            await function.PostPhase(phase);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Phase phase)
        {
            var function = new PhaseData();
            phase.id = id;
            await function.PutPhase(phase);
            
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var function = new PhaseData();
            var phase = new Phase();
            phase.id = id;
            await function.DeletePhase(phase);  
        }

    }
}

