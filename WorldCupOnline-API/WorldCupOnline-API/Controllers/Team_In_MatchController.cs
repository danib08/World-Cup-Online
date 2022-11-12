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
    public class Team_In_MatchController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Team_In_MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get all teams that play matches and the match they play
        /// </summary>
        /// <returns>JSONResult with all matches played</returns>
        [HttpGet]
        public JsonResult GetTeams_In_Matches()
        {
            string query = @"exec proc_teamInMatch '',0,'Select'"; ///sql query

            DataTable table = new DataTable(); ///Create datatable
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");///Establish connection
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
        /// Method to get a specific team in a match
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="matchid"></param>
        /// <returns>Json of the required object</returns>
        [HttpGet("{teamid}/{matchid}")]
        public string GetTeam_In_Match(string teamid, int matchid)
        {
            ///Created labels
            string lbl_teamid;
            string lbl_matchid;

            ///SQL Query
            string query = @"
                            exec proc_teamInMatch @teamid,@matchid,'Select One'";
            DataTable table = new DataTable();///Created table to store data
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))///Command with query and connection
                {
                    ///Added parameters
                    myCommand.Parameters.AddWithValue("@teamid", teamid);
                    myCommand.Parameters.AddWithValue("@matchid", matchid);
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
                lbl_teamid = row["teamid"].ToString();
                lbl_matchid = row["matchid"].ToString();

                ///Creation of the JSON
                var data = new JObject(new JProperty("teamid", lbl_teamid), new JProperty("matchid", lbl_matchid));
                

                return data.ToString(); ///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString(); ///Return message if table is empty
            }

        }

        /// <summary>
        /// Method to assign a team to a match
        /// </summary>
        /// <param name="team_In_Match"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostTeam_In_Match(Team_In_Match team_In_Match)
        {
            ///SQL Query
            string query = @"
                             exec proc_teamInMatch @teamid,@matchid,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@teamid", team_In_Match.teamid);
                myCommand.Parameters.AddWithValue("@matchid", team_In_Match.matchid);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection
            }

            return new JsonResult(table); ///Returns table with info

        }


        /// <summary>
        /// Method to delete a team in a match
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="matchid"></param>
        /// <returns></returns>
        [HttpDelete("{teamid}/{matchid}")]
        public ActionResult DeleteTeam_In_Match(string teamid, int matchid)
        {
            ///SQL Query
            string query = @"
                            exec proc_teamInMatch @teamid,@matchid,'Delete'
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))///Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon)) ///Command with query and connection
                {
                    myCommand.Parameters.AddWithValue("@teamid", teamid);
                    myCommand.Parameters.AddWithValue("@matchid", matchid);
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
