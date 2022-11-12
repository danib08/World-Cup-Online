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
    public class PhaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public PhaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get all created phases
        /// </summary>
        /// <returns>JSONResult with all phases</returns>
        [HttpGet]
        public JsonResult GetPhases()
        {
            string query = @"exec proc_phase 0,'','','Select'"; ///sql query

            DataTable table = new DataTable(); ///Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline"); ///Establish connection
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
                column.ColumnName = ti.ToLower(column.ColumnName); ///Make all lowercase to avoid conflicts with communication
            }

            return new JsonResult(table); ///Return JSON Of the data table
        }

        /// <summary>
        /// Method to get one phase by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Json of the required phase</returns>
        [HttpGet("{id}")]
        public string GetPhase(int id)
        {
            ///Created labels
            string lbl_id;
            string lbl_name;
            string lbl_tournamentid;

            ///SQL Query
            string query = @"
                            exec proc_phase @id,'','','Select'";
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
                lbl_tournamentid = row["tournamentID"].ToString();

                ///Creation of the JSON
                var data = new JObject(new JProperty("id", lbl_id), new JProperty("name", lbl_name),
                   new JProperty("tournamentID", float.Parse(lbl_tournamentid)));

                return data.ToString(); ///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString(); ///Return message if table is empty
            }
        }

        /// <summary>
        /// Method to create phases
        /// </summary>
        /// <param name="phase"></param>
        /// <returns>JSON of the phase created</returns>
        [HttpPost]
        public JsonResult PostPhase(Phase phase)
        {
            ///SQL Query
            string query = @"
                             exec proc_phase @id,@name,@tournamentid,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@id", phase.id);
                myCommand.Parameters.AddWithValue("@name", phase.name);
                myCommand.Parameters.AddWithValue("@tournamentid", phase.tournamentID);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection

            }

            return new JsonResult(table); ///Returns table with info
        }

        /// <summary>
        /// Method to update a phase
        /// </summary>
        /// <param name="phase"></param>
        /// <returns>Action result of the query</returns>
        [HttpPut]
        public ActionResult PutPhase(Phase phase)
        {
            ///SQL Query
            string query = @"
                             exec proc_phase @id,@name,@tournamentid,'Update'
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
                    myCommand.Parameters.AddWithValue("@id", phase.id);
                    myCommand.Parameters.AddWithValue("@name", phase.name);
                    myCommand.Parameters.AddWithValue("@tournamentID", phase.tournamentID);

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
        public ActionResult DeletePhase(int id)
        {
            ///SQL Query
            string query = @"
                            exec proc_phase @id,'','','Delete'
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
