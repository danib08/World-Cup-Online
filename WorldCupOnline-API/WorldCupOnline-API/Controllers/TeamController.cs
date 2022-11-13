using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Newtonsoft.Json.Linq;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Data;
using System.Reflection.Metadata.Ecma335;

namespace WorldCupOnline_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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


        [HttpGet]
        public async Task<ActionResult<List<Team>>> Get()
        {
            var function = new TeamData();

            var list = await function.GetTeams();
            return list;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Team>>> GetOne(string id)
        {
            var function = new TeamData();
            var team = new Team();
            team.id = id;
            var list = await function.GetOneTeam(team);
            return list;
        }

        //Get types falta


        [HttpPost]
        public async Task Post([FromBody] Team team)
        {
            var function = new TeamData();
            await function.PostTeams(team);
        }

        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Team team)
        {
            var function = new TeamData();
            team.id = id;
            await function.PutTeam(team);
            
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var function = new TeamData();
            var team = new Team();
            team.id = id;
            await function.DeleteTeam(team);  
        }

        /// <summary>
        /// Method to get all created types of teams
        /// </summary>
        /// <returns>returns>JSONResult with type of teams</returns>
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

