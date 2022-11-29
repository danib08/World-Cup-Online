using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;

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

        [HttpGet]
        public async Task<ActionResult<List<Match>>> Get()
        {
            var function = new MatchData();

            var list = await function.GetMatches();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Match>>> GetOne(int id)
        {
            var function = new MatchData();
            var match = new Match();
            match.id = id;
            var list = await function.GetOneMatch(match);
            return list;
        }

        [HttpPost]
        public async Task Post([FromBody] Match match)
        {
            var function = new MatchData();
            await function.PostMatch(match);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Match match)
        {
            var function = new MatchData();
            match.id = id;
            await function.PutMatch(match);
            
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var function = new MatchData();
            var match = new Match();
            match.id = id;
            await function.DeleteMatch(match);  
        }

        [HttpPost("postTeamInMatch")]
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
  
    }
}
