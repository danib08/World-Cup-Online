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
    public class Player_In_TeamController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Established configuration for controller to get connection
        /// </summary>
        /// <param name="configuration"></param>
        public Player_In_TeamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetPlayers_In_Teams()
        {
            string query = @"exec proc_player_In_Team '','',0,'Select'";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            foreach (DataColumn column in table.Columns)
            {
                column.ColumnName = ti.ToLower(column.ColumnName);
            }

            return new JsonResult(table);
        }

        /// <summary>
        /// Method to get player in team
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        [HttpGet("{playerid}/{teamid}")]
        public string GetPlayer_In_Team(string playerid, string teamid)
        {
            string lbl_playerid;
            string lbl_teamid;

            //SQL Query
            string query = @"
                            exec proc_player_In_Team @teamid,@playerid,0,'Select One'";
            DataTable table = new DataTable();//Created table to store data
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection created
            {
                myCon.Open();//Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))//Command with query and connection
                {
                    //Added parameters
                    myCommand.Parameters.AddWithValue("@playerid", playerid);
                    myCommand.Parameters.AddWithValue("@teamid", teamid);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); //Load data to table
                    myReader.Close();
                    myCon.Close(); //Close connection
                }
            }

            if (table.Rows.Count > 0)
            {

                DataRow row = table.Rows[0];

                lbl_playerid = row["playerid"].ToString();
                lbl_teamid = row["teamid"].ToString();

                var data = new JObject(new JProperty("teamid", lbl_teamid), new JProperty("playerid", lbl_playerid));

                return data.ToString();
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString();
            }
        }

        /// <summary>
        /// Method to create a player in team
        /// </summary>
        /// <param name="player_In_Team"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostPlayer_In_Team(Player_In_Team player_In_Team)
        {
            //SQL Query
            string query = @"
                             exec proc_player_In_Team @teamid,@playerid,@jerseynum,'Insert'
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection stablished
            {
                myCon.Open(); //Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                //Parameters added with values
                myCommand.Parameters.AddWithValue("@teamid", player_In_Team.teamid);
                myCommand.Parameters.AddWithValue("@playerid", player_In_Team.playerid);
                myCommand.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();//Closed connection
            }

            return new JsonResult(table); //Returns table with info
        }

        /// <summary>
        /// Method update a player in team
        /// </summary>
        /// <param name="player_In_Team"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult PutPlayer_In_Team(Player_In_Team player_In_Team)
        {
            //SQL Query
            string query = @"
                             exec proc_player_In_Team @teamid,@playerid,@jerseynum,'Update'
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
                    myCommand.Parameters.AddWithValue("@playerid", player_In_Team.playerid);
                    myCommand.Parameters.AddWithValue("@teamid", player_In_Team.teamid);
                    myCommand.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();//Closed connection
                }
            }
            return Ok(); //Returns acceptance
        }

        /// <summary>
        /// Method to delete a player in team
        /// </summary>
        /// <param name="playerid"></param>
        /// <param name="teamid"></param>
        /// <returns></returns>
        [HttpDelete("{playerid}/{teamid}")]
        public ActionResult DeletePlayer_In_Team(string playerid, string teamid)
        {
            //SQL Query
            string query = @"
                            exec proc_player_In_Team @teamid,@playerid,0,'Delete'
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection created
            {
                myCon.Open();//Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon)) //Command with query and connection
                {
                    myCommand.Parameters.AddWithValue("@playerid", playerid);
                    myCommand.Parameters.AddWithValue("@teamid", teamid);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();//Closed connection
                }
            }
            return Ok(); //Returns acceptance
        }
    }
}
