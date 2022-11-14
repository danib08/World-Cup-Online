using Microsoft.AspNetCore.Mvc;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

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

        /*[HttpPost("Auth")]
        public IActionResult AuthUser(Auth auth)
        {
            byte[] bytesPassword = Encoding.ASCII.GetBytes(auth.password);
            byte[] hash;

            using (SHA512 hasher = SHA512.Create())
            {
                hash = hasher.ComputeHash(bytesPassword);
            }

            string encrypted = Convert.ToBase64String(hash);


            //SQL Query
            string query = @"exec proc_users '','','',@email,'','','','Auth'";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@email", auth.email);
          
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection

            }
            DataRow row = table.Rows[0];
            string lbl_pwd = row["password"].ToString();
            string lbl_username = row["username"].ToString();

            if (lbl_pwd == encrypted)
            {
                var data = new JObject(new JProperty("username", lbl_username));
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }*/

    }
}



