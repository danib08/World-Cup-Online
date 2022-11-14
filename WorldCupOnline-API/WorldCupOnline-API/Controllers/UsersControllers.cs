using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using WorldCupOnline_API.Models;
using System.Security.Cryptography;
using System.Text;

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

        /// <summary>
        /// Method to get all created users
        /// </summary>
        /// <returns>JSONResult with all users</returns>
        [HttpGet]
        public JsonResult GetUsers()
        {
            string query = @"exec proc_users '','','','','','','','Select'"; ///sql query

            DataTable table = new DataTable(); //Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); ///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ///Data is loaded into table
                    myReader.Close();
                    myCon.Close(); ///Closed connection
                }
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            foreach (DataColumn column in table.Columns)
            {
                column.ColumnName = ti.ToLower(column.ColumnName); ///Make all lowercase to avoid conflicts with communication
            }

            return new JsonResult(table); ///Return JSON Of the data table
        }

        /// <summary>
        /// Method to get one user by its username
        /// </summary>
        /// <param username="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public string GetUser(string username)
        {
            ///Created label
            string lbl_username;
            string lbl_name;
            string lbl_lastname;
            string lbl_email;
            string lbl_countryid;
            string lbl_birthdate;

            ///SQL Query
            string query = @"
                            exec proc_users @username,'','','','','','','Select One'";

            DataTable table = new DataTable();///Created table to store data
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))///Command with query and connection
                {
                    ///Added parameters
                    myCommand.Parameters.AddWithValue("@username", username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ///Load data to table
                    myReader.Close();
                    myCon.Close(); ///Close connection
                }
            }

            ///Verify if table is empty
            if (table.Rows.Count > 0)
            {

                DataRow row = table.Rows[0];

                ///Manipulation of every row of datatable and parse them to string
                lbl_username = row["username"].ToString();
                lbl_name = row["name"].ToString();
                lbl_lastname = row["lastname"].ToString();
                lbl_email = row["email"].ToString();
                lbl_countryid = row["countrid"].ToString();
                lbl_birthdate = row["birthdate"].ToString();


                ///Creation of the JSON
                var data = new JObject(new JProperty("username", lbl_username), new JProperty("name", lbl_name),
                    new JProperty("lastname", lbl_lastname), new JProperty("email", lbl_email),
                    new JProperty("countryid", lbl_countryid), new JProperty("birthdate", lbl_birthdate));

                return data.ToString(); ///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString(); ///Return message if table is empty
            }

        }

        /// <summary>
        /// Method to create users
        /// </summary>
        /// <param users=""></param>
        /// <returns>JSON of the type created</returns>
        [HttpPost]
        public JsonResult PostUser(Users user)
        {
            byte[] bytesPassword = Encoding.ASCII.GetBytes(user.password);
            byte[] hash;

            using (SHA512 shaM = SHA512.Create())
            {
                hash = shaM.ComputeHash(bytesPassword);
            }

            string encrypted = Convert.ToBase64String(hash);


            //SQL Query
            string query = @"
                             exec proc_users @username,@name,@lastname,@email,@countryid,@birthdate,@password,'Insert'
                           ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@username", user.username);
                myCommand.Parameters.AddWithValue("@name", user.name);
                myCommand.Parameters.AddWithValue("@lastname", user.lastname);
                myCommand.Parameters.AddWithValue("@email", user.email);
                myCommand.Parameters.AddWithValue("@countryid", user.countryid);
                myCommand.Parameters.AddWithValue("@birthdate", user.birthdate);
                myCommand.Parameters.AddWithValue("@password", encrypted);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection

            }

            return new JsonResult(table); ///Returns table with info
        }

        /// <summary>
        /// Method to delete a user by its username
        /// </summary>
        /// <param username="username"></param>
        /// <returns></returns>
        [HttpDelete("{username}")]
        public ActionResult DeleteUser(string username)
        {
            ///SQL Query
            string query = @"
                            exec proc_users @username,'','','','','','','Delete'";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon)) ///Command with query and connection
                {
                    myCommand.Parameters.AddWithValue("@username", username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();///Closed connection
                }
            }
            return Ok(); ///Returns acceptance
        }
    }
}
