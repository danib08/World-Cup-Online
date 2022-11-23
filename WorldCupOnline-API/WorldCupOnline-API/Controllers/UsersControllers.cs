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

        /// <summary>
        /// Service to get all Users
        /// </summary>
        /// <returns>List of Users</returns>
        [HttpGet]
        public async Task<ActionResult<List<Users>>> Get()
        {
            return await _funct.GetUsers();
        }

        /// <summary>
        /// Service to get one User
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Users</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetOne(string id)
        {
            return await _funct.GetOneUser(id);
        }

        /// <summary>
        /// Service to post Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody] Users user)
        {
            await _funct.CreateUsers(user);
        }

        /// <summary>
        /// Service to edit Users
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Users user)
        {
            await _funct.EditUser(id, user);
        }

        /// <summary>
        /// Service to delete Users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _funct.DeleteUser(id);
        }

        /// <summary>
        /// Service to authenticate users
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("Auth")]
        public IActionResult Auth(Auth auth)
        {
            string result = _funct.AuthUser(auth).Result;

            ///Validation of non existance
            if (result.Equals("")) {
                return BadRequest();
            }
            else ///If exists sends Ok state result
            {
                var data = new JObject(new JProperty("username", result));
                return Ok(data);
            }
        }
    }
}



