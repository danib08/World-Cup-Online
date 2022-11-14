using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using WorldCupOnline_API.Models;
using System.Security.Cryptography;
using System.Text;
using WorldCupOnline_API.Data;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private SHA512 _hashAlgorithm;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _hashAlgorithm = SHA512.Create();
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> Get()
        {
            var function = new UserData();

            var list = await function.GetUsers();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Users>>> GetOne(string id)
        {
            var function = new UserData();
            var user = new Users();
            user.username = id;
            var list = await function.GetOneUser(user);
            return list;
        }

        [HttpPost]
        public async Task Post([FromBody] Users user)
        {
            var function = new UserData();
            await function.PostUsers(user);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Users user)
        {
            var function = new UserData();
            user.username = id;
            await function.PutUser(user);

        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var function = new UserData();
            var user = new Users();
            user.username = id;
            await function.DeleteUser(user);
        }

        /// Method to authenticate users
        /// </summary>
        /// <param auth=""></param>
        /// <returns></returns>
        [HttpPost("Auth")]
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
        }

    }
}



