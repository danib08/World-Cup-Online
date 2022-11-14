using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly StateData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public StateController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new StateData();
        }

        [HttpGet]
        public async Task<ActionResult<List<State>>> Get()
        {
            return await _funct.GetStates();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<State>>> GetOne(int id)
        {
            return await _funct.GetOneState(id);
        }


        [HttpPost]
        public async Task Post([FromBody] State state)
        {
            await _funct.CreateState(state);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] State state)
        {
            await _funct.EditState(id, state);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteState(id);
        }
    }
}