using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Data;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Interfaces;

namespace WorldCupOnline_API.Repositories
{
    [Route("api/Users")]
    [ApiController]
    public class UserRepository : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserData _funct;

        /// <summary>
        /// Establish configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public UserRepository(IConfiguration configuration)
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
        /// Service to get all Countries
        /// </summary>
        /// <returns>List of ValueStringBody</returns>
        [HttpGet("Country")]
        public async Task<ActionResult<List<ValueStringBody>>> GetCountries()
        {
            return await _funct.GetCountries();
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
        /// Service to authenticate users
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("Auth")]
        public IActionResult Auth(Auth auth)
        {
            string result = _funct.AuthUser(auth).Result;

            ///Validation of non existance
            if (result.Equals(""))
            {
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
