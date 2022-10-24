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
    public class Team_In_TournamentController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Team_In_TournamentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get all created teams in all tournaments
        /// </summary>
        /// <returns>JSONResult with all teams in tournaments</returns>
        [HttpGet]
        public JsonResult GetTeams_In_Tournament()
        {
            string query = @"exec proc_teamInTournament '','','Select'";///sql query

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

            return new JsonResult(table); ///Return JSON Of the data table
        }

        /// <summary>
        /// Method to get one team in a tournament by its ids
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="tournamentid"></param>
        /// <returns></returns>
        [HttpGet("{teamid}/{tournamentid}")]
        public string GetTeam_In_Tournament(string teamid, string tournamentid)
        {
            ///Created labels
            string lbl_teamid;
            string lbl_tournamentid;

            ///SQL Query
            string query = @"
                            exec proc_teamInTournament @teamid,@tournamentid,'Select One'";
            DataTable table = new DataTable();///Created table to store data
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))//Command with query and connection
                {
                    ///Added parameters
                    myCommand.Parameters.AddWithValue("@teamid", teamid);
                    myCommand.Parameters.AddWithValue("@tournamentid", tournamentid);
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
                lbl_tournamentid = row["tournamentid"].ToString();

                ///Creation of the JSON
                var data = new JObject(new JProperty("teamid", lbl_teamid), new JProperty("tournamentid", lbl_tournamentid));

                return data.ToString();///Return created JSON
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString(); ///Return message if table is empty
            }
        }

        /// <summary>
        /// Method to assign team to a tournament
        /// </summary>
        /// <param name="team_In_Tournament"></param>
        /// <returns>JSON of the team assigned</returns>
        [HttpPost]
        public JsonResult PostTeam_In_Tournament(Team_In_Tournament team_In_Tournament)
        {
            ///SQL Query
            string query = @"
                             exec proc_teamInTournament @teamid,@tournamentid,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection stablished
            {
                myCon.Open(); ///Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                ///Parameters added with values
                myCommand.Parameters.AddWithValue("@teamid", team_In_Tournament.teamid);
                myCommand.Parameters.AddWithValue("@tournamentid", team_In_Tournament.tournamentid);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();///Closed connection
            }

            return new JsonResult(table); ///Returns table with info

        }


        [HttpPut]
        public ActionResult PutTeam_In_Tournament(Team_In_Tournament team_In_Tournament)
        {
            //SQL Query
            string query = @"
                             exec proc_teamInTournament @teamid,@tournamentid,'Update'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection started
            {
                myCon.Open(); //Connection closed
                using (SqlCommand myCommand = new SqlCommand(query, myCon))//Sql command with query and connection
                {
                    //Added parameters
                    myCommand.Parameters.AddWithValue("@teamid", team_In_Tournament.teamid);
                    myCommand.Parameters.AddWithValue("@tournamentid", team_In_Tournament.tournamentid);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();//Closed connection
                }
            }
            return Ok(); //Returns acceptance
        }

        /// <summary>
        /// Method to delete a phase by its id
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="tournamentid"></param>
        /// <returns></returns>
        [HttpDelete("{teamid}/{tournamentid}")]
        public ActionResult DeleteTeam_In_Tournament(string teamid, string tournamentid)
        {
            ///SQL Query
            string query = @"
                            exec proc_teamInTournament @teamid,@tournamentid,'Delete'
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection created
            {
                myCon.Open();///Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon)) //Command with query and connection
                {
                    myCommand.Parameters.AddWithValue("@teamid", teamid);
                    myCommand.Parameters.AddWithValue("@tournamentid", tournamentid);
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
