using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using Newtonsoft.Json.Linq;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _funct = new UserData();
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> Get()
        {
            return await _funct.GetUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetOne(string id)
        {
            return await _funct.GetOneUser(id);
        }

        [HttpPost]
        public async Task Post([FromBody] Users user)
        {
            await _funct.CreateUsers(user);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Users user)
        {
            await _funct.EditUser(id, user);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeleteUser(id);
        }

        [HttpPost("Auth")]
        public IActionResult Auth(Auth auth)
        {
            string result = _funct.AuthUser(auth).Result;

            if (result.Equals("")) {
                return BadRequest();
            }
            else
            {
                var data = new JObject(new JProperty("username", result));
                return Ok(data);
            }
        }
    }
}



