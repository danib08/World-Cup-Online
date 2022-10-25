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
    
    public class MatchController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get all created matches
        /// </summary>
        /// <returns>JSONResult with all matches</returns>
        [HttpGet]
        public JsonResult GetMatches()
        {
            string query = @"exec proc_match 0,'','','','',0,'',0,'Select'"; ///sql query

            DataTable table = new DataTable(); ///Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline"); ///Establish connection
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
        /// Method to get one match by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Json of the required match</returns>
        [HttpGet("{id}")]
        public string GetMatch(int id)
        {
            ///Created labels
            string lbl_id;
            string lbl_startDate;
            string lbl_startTime;
            string lbl_score;
            string lbl_location;
            string lbl_stateId;
            string lbl_tournamentId;
            string lbl_phaseId;


            ///SQL Query
            string query = @"exec proc_match @id,'','','','',0,'',0,'Select One'"; 
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
                lbl_startDate = row["startdate"].ToString();
                lbl_startTime = row["starttime"].ToString();
                lbl_score = row["score"].ToString();
                lbl_location = row["location"].ToString();
                lbl_stateId = row["stateid"].ToString();
                lbl_tournamentId = row["tournamentid"].ToString();
                lbl_phaseId = row["phaseid"].ToString();

                ///Creation of the JSON
                var data = new JObject(new JProperty("id", lbl_id), new JProperty("startdate", DateTime.Parse(lbl_startDate)), 
                   new JProperty("starttime", DateTime.Parse(lbl_startTime)), new JProperty("score", lbl_score),new JProperty("location", lbl_location),
                   new JProperty("stateid", lbl_stateId), new JProperty("tournamentid", lbl_tournamentId), new JProperty("phaseid", lbl_phaseId));

                return data.ToString(); ///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString(); ///Return message if table is empty
            }

        }

        /// <summary>
        /// Method to create matches
        /// </summary>
        /// <param name="match"></param>
        /// <returns>JSON of the match created</returns>
        [HttpPost]
        public JsonResult CreateMatch(MatchCreator creator)
        {

            ///SQL Query
            string query = @"
                             exec proc_match '',@startdate,@starttime,@score,@location,@stateid,@tournamentid,@phaseid,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@startdate", creator.startdate);
                myCommand.Parameters.AddWithValue("@starttime", creator.starttime);
                myCommand.Parameters.AddWithValue("@score", "0-0");
                myCommand.Parameters.AddWithValue("@location", creator.location);
                myCommand.Parameters.AddWithValue("@stateid", 1);
                myCommand.Parameters.AddWithValue("@tournamentid", creator.tournamentid);
                myCommand.Parameters.AddWithValue("@phaseid", creator.phaseid);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection

            }

            return new JsonResult(table); ///Returns table with info

        }

        /// <summary>
        /// Method to update a match
        /// </summary>
        /// <param name="match"></param>
        /// <returns>Action result of the query</returns>
        [HttpPut]
        public ActionResult PutMatch(Match match)
        {
            //SQL Query
            string query = @"
                             exec proc_match @id,@startdate,@starttime,@score,@location,@stateid,@tournamentid,@phaseid'Update'
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
                    myCommand.Parameters.AddWithValue("@id", match.id);
                    myCommand.Parameters.AddWithValue("@startdate", match.startdate);
                    myCommand.Parameters.AddWithValue("@starttime", match.starttime);
                    myCommand.Parameters.AddWithValue("@score", match.score);
                    myCommand.Parameters.AddWithValue("@location", match.location);
                    myCommand.Parameters.AddWithValue("@stateid", match.stateid);
                    myCommand.Parameters.AddWithValue("@tournamentid", match.tournamentid);
                    myCommand.Parameters.AddWithValue("@phaseid", match.phaseid);


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();///Closed connection
                }
            }
            return Ok(); ///Returns acceptance
        }

        /// <summary>
        /// Method to delete a match by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Action result of the query</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteMatch(int id)
        {
            ///SQL Query
            string query = @"
                            exec proc_match @id,'','','','',0,'',0,'Delete'
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
