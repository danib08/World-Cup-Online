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

        public MatchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetMatches()
        {
            string query = @"stored procedure";

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

        [HttpGet("{id}")]
        public string GetMatch(int id)
        {

            string lbl_id;
            string lbl_startDate;
            string lbl_startTime;
            string lbl_score;
            string lbl_location;
            string lbl_state;
            string lbl_tournamentId;


            //SQL Query
            string query = @"
                            stored procedure";
            DataTable table = new DataTable();//Created table to store data
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection created
            {
                myCon.Open();//Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon))//Command with query and connection
                {
                    //Added parameters
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); //Load data to table
                    myReader.Close();
                    myCon.Close(); //Close connection
                }
            }

            if (table.Rows.Count > 0)
            {

                DataRow row = table.Rows[0];


                lbl_id = row["id"].ToString();
                lbl_startDate = row["startdate"].ToString();
                lbl_startTime = row["starttime"].ToString();
                lbl_score = row["score"].ToString();
                lbl_location = row["location"].ToString();
                lbl_state = row["state"].ToString();
                lbl_tournamentId = row["tournamentid"].ToString();


                var data = new JObject(new JProperty("id", lbl_id), new JProperty("startdate", DateTime.Parse(lbl_startDate)), 
                   new JProperty("starttime", DateTime.Parse(lbl_startTime)), new JProperty("score", lbl_score),new JProperty("location", lbl_location),
                   new JProperty("state",lbl_state), new JProperty("tournamentid", lbl_tournamentId));

                return data.ToString();
            }
            else
            {
                var data = new JObject(new JProperty("Existe", "no"));
                return data.ToString();
            }

        }


        [HttpPost]
        public JsonResult PostMatch(Match match)
        {


            //SQL Query
            string query = @"
                             stored procedure
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection stablished
            {
                myCon.Open(); //Opened connection
                SqlCommand myCommand = new SqlCommand(query, myCon);

                //Parameters added with values
                myCommand.Parameters.AddWithValue("@id", match.id);
                myCommand.Parameters.AddWithValue("@startdate", match.startdate);
                myCommand.Parameters.AddWithValue("@starttime", match.starttime);
                myCommand.Parameters.AddWithValue("@score", match.score);
                myCommand.Parameters.AddWithValue("@location", match.location);
                myCommand.Parameters.AddWithValue("@state", match.state);
                myCommand.Parameters.AddWithValue("@tournamentid", match.tournamentid);


                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();//Closed connection

            }

            return new JsonResult(table); //Returns table with info

        }


        [HttpPut]
        public ActionResult PutMatch(Match match)
        {
            //SQL Query
            string query = @"
                             stored procedures
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
                    myCommand.Parameters.AddWithValue("@id", match.id);
                    myCommand.Parameters.AddWithValue("@startdate", match.startdate);
                    myCommand.Parameters.AddWithValue("@starttime", match.starttime);
                    myCommand.Parameters.AddWithValue("@score", match.score);
                    myCommand.Parameters.AddWithValue("@location", match.location);
                    myCommand.Parameters.AddWithValue("@state", match.state);
                    myCommand.Parameters.AddWithValue("@tournamentid", match.tournamentid);


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();//Closed connection
                }
            }
            return Ok(); //Returns acceptance
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMatch(int id)
        {
            //SQL Query
            string query = @"
                            stored procedure
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("WorldCupOnline");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))//Connection created
            {
                myCon.Open();//Open connection
                using (SqlCommand myCommand = new SqlCommand(query, myCon)) //Command with query and connection
                {
                    myCommand.Parameters.AddWithValue("@id", id);
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
