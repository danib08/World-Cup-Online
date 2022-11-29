using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public StateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<State>>> Get()
        {
            var function = new StateData();

            var list = await function.GetStates();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<State>>> GetOne(int id)
        {
            var function = new StateData();
            var state = new State();
            state.id = id;
            var list = await function.GetOneState(state);
            return list;
        }


        [HttpPost]
        public async Task Post([FromBody] State state)
        {
            var function = new StateData();
            await function.PostState(state);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] State state)
        {
            var function = new StateData();
            state.id = id;
            await function.PutState(state);

        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var function = new StateData();
            var state = new State();
            state.id = id;
            await function.DeleteState(state);
        }
    }
}

