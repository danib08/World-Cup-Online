using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Newtonsoft.Json.Linq;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TeamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get all created reams
        /// </summary>
        /// <returns>returns>JSONResult with all phases</returns>
        [HttpGet]
        public JsonResult GetTeams()
        {
            string query = @"exec proc_team '','','',0,'Select WebApp'";///sql query

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
                    myCon.Close(); ///Closed connection
                }
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            foreach (DataColumn column in table.Columns)
            {
                column.ColumnName = ti.ToLower(column.ColumnName);///Make all lowercase to avoid conflicts with communication
            }

            return new JsonResult(table);///Return JSON Of the data table
        }

        /// <summary>
        /// Method to get one team by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Json of the required team</returns>
        [HttpGet("{id}")]
        public string GetTeam(string id)
        {
            ///Created labels
            string lbl_id;
            string lbl_name;
            string lbl_confederation;
            string lbl_type;

            ///SQL Query
            string query = @"
                            exec proc_team @id,'','',0,'Select One'";
            DataTable table = new DataTable();///Created table to store data
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection created
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
                lbl_confederation = row["confederation"].ToString();
                lbl_type = row["typeid"].ToString();

                ///Creation of the JSON
                var data = new JObject(new JProperty("id", lbl_id), new JProperty("name", lbl_name),
                   new JProperty("confederation", lbl_confederation),
                   new JProperty("typeid", lbl_type));

                return data.ToString();///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString(); ///Return message if table is empty
            }
        }

        /// <summary>
        /// Method to create teams
        /// </summary>
        /// <param name="team"></param>
        /// <returns>JSON of the team created</returns>
        [HttpPost]
        public JsonResult PostTeam(Team team)
        {
            ///SQL Query
            string query = @"
                             exec proc_team @id,@name,@confederation,@typeid,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@id", team.id);
                myCommand.Parameters.AddWithValue("@name", team.name);
                myCommand.Parameters.AddWithValue("@confederation", team.confederation);
                myCommand.Parameters.AddWithValue("@typeid", team.typeid);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection

            }

            return new JsonResult(table); ///Returns table with info
        }

        /// <summary>
        /// Method to update a team
        /// </summary>
        /// <param name="team"></param>
        /// <returns>Action result of the query</returns>
        [HttpPut]
        public ActionResult PutTeam(Team team)
        {
            ///SQL Query
            string query = @"
                             exec proc_team @id,@name,@confederation,@typeid,'Updates'
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
                    myCommand.Parameters.AddWithValue("@id", team.id);
                    myCommand.Parameters.AddWithValue("@name", team.name);
                    myCommand.Parameters.AddWithValue("@confederation", team.confederation);
                    myCommand.Parameters.AddWithValue("@typeid", team.typeid);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();///Closed connection
                }
            }
            return Ok(); ///Returns acceptance
        }

        /// <summary>
        /// Method to delete a team by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Action result of the query</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteTeam(string id)
        {
            ///SQL Query
            string query = @"
                            exec proc_team @id,'','',0,'Delete'
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


        /// <summary>
        /// Method to get all created reams
        /// </summary>
        /// <returns>returns>JSONResult with all phases</returns>
        [HttpGet("Type/{type}")]
        public JsonResult GetTeamsByType(int type)
        {
            string query = @"exec proc_team '','','',@type,'Select Type'";///sql query

            DataTable table = new DataTable(); ///Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@type", type);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ///Data is loaded into table
                    myReader.Close();
                    myCon.Close(); ///Closed connection
                }
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            foreach (DataColumn column in table.Columns)
            {
                column.ColumnName = ti.ToLower(column.ColumnName);///Make all lowercase to avoid conflicts with communication
            }

            return new JsonResult(table);///Return JSON Of the data table
        }
    }
}

