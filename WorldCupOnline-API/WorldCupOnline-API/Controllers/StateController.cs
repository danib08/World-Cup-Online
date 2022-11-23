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

        /// <summary>
        /// Service to get State
        /// </summary>
        /// <returns>List of State</returns>
        [HttpGet]
        public async Task<ActionResult<List<State>>> Get()
        {
            return await _funct.GetStates();
        }

        /// <summary>
        /// Service to get one State
        /// </summary>
        /// <param name="id"></param>
        /// <returns>State</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetOne(int id)
        {
            return await _funct.GetOneState(id);
        }

        /// <summary>
        /// Service to insert State
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] State state)
        {
            await _funct.CreateState(state);
        }

        /// <summary>
        /// Service to edit State
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] State state)
        {
            await _funct.EditState(id, state);
        }

        /// <summary>
        /// Service to delete State
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _funct.DeleteState(id);
        }
    }
}