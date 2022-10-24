using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public PlayerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get all created players
        /// </summary>
        /// <returns>JSONResult with all players</returns>
        [HttpGet]
        public JsonResult GetPlayers()
        {
            string query = @"exec proc_player '','','','','Select'";///sql query

            DataTable table = new DataTable(); ///Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ///Data is loaded into table
                    myReader.Close();
                    myCon.Close();///Closed connection
                }
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            foreach (DataColumn column in table.Columns)
            {
                column.ColumnName = ti.ToLower(column.ColumnName);///Make all lowercase to avoid conflicts with communication
            }

            return new JsonResult(table); ///Return JSON Of the data table
        }

        /// <summary>
        /// Method to get one player by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Json of the required phase</returns>
        [HttpGet("{id}")]
        public string GetPlayer(string id)
        {
            ///Created labels
            string lbl_id;
            string lbl_name;
            string lbl_lastname;
            string lbl_position;

            ///SQL Query
            string query = @"
                            exec proc_player @id,'','','','Select One'";
            DataTable table = new DataTable();///Created table to store data
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))///Command with query and connection
                {
                    ///Added parameters
                    myCommand.Parameters.AddWithValue("@id", id);
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
                lbl_id = row["id"].ToString();
                lbl_name = row["name"].ToString();
                lbl_lastname = row["lastname"].ToString();
                lbl_position = row["position"].ToString();

                ///Creation of the JSON
                var data = new JObject(new JProperty("id", lbl_id), new JProperty("name", lbl_name),
                    new JProperty("lastname", lbl_lastname), new JProperty("position", lbl_position));

                return data.ToString();///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString();///Return message if table is empty
            }

        }

        /// <summary>
        /// Method to create players
        /// </summary>
        /// <param name="player"></param>
        /// <returns>JSON of the player created</returns>
        [HttpPost]
        public JsonResult PostPlayer(Player player)
        {
            ///SQL Query
            string query = @"
                             exec proc_player @id,@name,@lastname,@position,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@id", player.id);
                myCommand.Parameters.AddWithValue("@name", player.name);
                myCommand.Parameters.AddWithValue("@lastname", player.lastname);
                myCommand.Parameters.AddWithValue("@position", player.position);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection
            }

            return new JsonResult(table); ///Returns table with info

        }

        /// <summary>
        /// Method to update a player
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Action result of the query</returns>
        [HttpPut]
        public ActionResult PutPlayer(Player player)
        {
            ///SQL Query
            string query = @"
                             exec proc_player @id,@name,@lastname,@position,'Update'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection started
            {
                myCon.Open(); ///Connection closed
                using (SqlCommand myCommand = new SqlCommand(query, myCon))///Sql command with query and connection
                {
                    ///Added parameters
                    myCommand.Parameters.AddWithValue("@id", player.id);
                    myCommand.Parameters.AddWithValue("@name", player.name);
                    myCommand.Parameters.AddWithValue("@lastname", player.lastname);
                    myCommand.Parameters.AddWithValue("@position", player.position);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();///Closed connection
                }
            }
            return Ok(); ///Returns acceptance
        }

        /// <summary>
        /// Method to delete a phase by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Action result of the query</returns>
        [HttpDelete("{id}")]
        public ActionResult DeletePlayer(string id)
        {
            ///SQL Query
            string query = @"
                            exec proc_player @id,'','','','Delete'
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon)) ///Command with query and connection
                {
                    myCommand.Parameters.AddWithValue("@id", id);
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
