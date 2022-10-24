using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public TournamentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get all created tournaments
        /// </summary>
        /// <returns>JSONResult with all tournaments</returns>
        [HttpGet]
        public JsonResult GetTournaments()
        {
            string query = @"exec proc_tournament '','','','','',0,'Select WebApp'"; ///sql query

            DataTable table = new DataTable(); ///Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");  ///Establish connection
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
            foreach(DataColumn column in table.Columns)
            {
                column.ColumnName = ti.ToLower(column.ColumnName); ///Make all lowercase to avoid conflicts with communication
            }

            return new JsonResult(table); ///Return JSON Of the data table
        }

        [HttpGet("Matches/{id}")]
        public JsonResult GetMatchesByTournament(string id)
        {
            string query = @"exec proc_tournament @id,'','','','',0,'Get Matches By Tourn'"; ///sql query

            DataTable table = new DataTable(); ///Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");  ///Establish connection
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); ///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
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
        /// Method to get one Tournament by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Json of the required tournaments</returns>
        [HttpGet("{id}")]
        public string GetTournament(string id)
        {
            ///Created labels
            string lbl_id;
            string lbl_name;
            string lbl_startdate;
            string lbl_enddate;
            string lbl_local;
            string lbl_description;

            ///SQL Query
            string query = @"
                            exec proc_tournament @id,'','','','',0,'Select One'";
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
                lbl_startdate = row["startDate"].ToString();
                lbl_enddate = row["endDate"].ToString();
                lbl_local = row["typeid"].ToString();
                lbl_description= row["description"].ToString();


                var data = new JObject(new JProperty("id", lbl_id), new JProperty("name", lbl_name),
                   new JProperty("startDate", DateTime.Parse(lbl_startdate)), new JProperty("endDate", DateTime.Parse(lbl_enddate)),
                   new JProperty("description", lbl_description), new JProperty("typeid", float.Parse(lbl_local)));

                return data.ToString(); ///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString(); ///Return message if table is empty
            }
        }

        /// <summary>
        /// Method to create tournament
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns>JSON of the tournament created</returns>
        [HttpPost]
        public JsonResult PostTournament(Tournament tournament)
        {
            

            ///SQL Query
            string query = @"
                             exec proc_tournament @id,@name,@startdate,@enddate,@description,@typeid,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@id", tournament.id);
                myCommand.Parameters.AddWithValue("@name", tournament.name);
                myCommand.Parameters.AddWithValue("@startdate", tournament.startDate);
                myCommand.Parameters.AddWithValue("@enddate", tournament.endDate);
                myCommand.Parameters.AddWithValue("@typeid", tournament.typeid);
                myCommand.Parameters.AddWithValue("@description", tournament.description);


                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection
            }

            return new JsonResult(table); ///Returns table with info

        }

        /// <summary>
        /// Method to update a tournament
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns>Action result of the query</returns>
        [HttpPut]
        public ActionResult PutTournament(Tournament tournament)
        {
            ///SQL Query
            string query = @"
                             exec proc_tournament @id,@name,@startdate,@enddate,@description,@typeid,'Update'
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
                    myCommand.Parameters.AddWithValue("@id", tournament.id);
                    myCommand.Parameters.AddWithValue("@name", tournament.name);
                    myCommand.Parameters.AddWithValue("@startdate", tournament.startDate);
                    myCommand.Parameters.AddWithValue("@enddate", tournament.endDate);
                    myCommand.Parameters.AddWithValue("@typeid", tournament.typeid);
                    myCommand.Parameters.AddWithValue("@description", tournament.description);


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();///Closed connection
                }
            }
            return Ok(); ///Returns acceptance
        }

        /// <summary>
        /// Method to delete a tournaments by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteTournament(string id)
        {
            ///SQL Query
            string query = @"
                            exec proc_tournament @id,'','','',0,'','Delete'
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
