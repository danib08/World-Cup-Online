using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Data;
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

        [HttpGet]
        public async Task<ActionResult<List<Tournament>>> Get()
        {
            var function = new TournamentData();

            var list = await function.GetTournament();
            return list;
        }

        /// <summary>
        /// Method to get one Tournament by its id
        /// </summary>
        /// <param id="id"></param>
        /// <returns>Json of the required tournaments</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Tournament>>> GetOne(int id)
        {
            var function = new TournamentData();
            var tournament = new Tournament();
            tournament.id = id;
            var list = await function.GetOneTournament(tournament);
            return list;
        }


        [HttpPost]
        public async Task Post([FromBody] TournamentCreator tournament)
        {
            var function = new TournamentData();
            var fun2 = new Team_In_TournamentData();
            await function.PostTournament(tournament);


            foreach(string teamid in tournament.teamsIds)
            {
                var team_in_tournament = new Team_In_Tournament { teamid = teamid, };
                await fun2.PostTeam_In_Tournament(team_in_tournament);
            }
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Tournament tournament)
        {
            var function = new TournamentData();
            tournament.id = id;
            await function.PutTournament(tournament);

        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var function = new TournamentData();
            var tournament = new Tournament();
            tournament.id = id;
            await function.DeleteTournament(tournament);
        }

        [HttpGet("{tournamentId}/Matches")]
        public async Task<ActionResult<List<MatchTournamentBody>>> GetMatchesTournament(int tournamentId)
        {
            var function = new TournamentData();
            var tournament = new Tournament();
            tournament.id = tournamentId;
            var list = await function.GetMatchesTournament(tournament);
            return list;
        }



        [HttpGet("Phases/{id}")]
        public async Task<ActionResult<List<PhasesBody>>> GetPhasesTournament(int id)
        {
            var function = new TournamentData();
            var tournament = new Tournament();
            tournament.id = id;
            var list = await function.GetPhasesTournament(tournament);
            return list;
        }

        [HttpGet("Teams/{tournamentId}")]
        public async Task<ActionResult<List<TeamTournamentBody>>> GetTeamsTournament(int tournamentId)
        {
            var function = new TournamentData();
            var tournament = new Tournament();
            tournament.id = tournamentId;
            var list = await function.GetTeamsTournament(tournament);
            return list;
        }
    }
}
        /**

        /// <summary>
        /// Create a team in tournament
        /// </summary>
        /// <param name="team_In_Tournament"></param>
        /// <returns></returns>
        [HttpPost("postTeamInTournament")]
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



        /// <summary>
        /// Method to create a phase in a tournament
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        [HttpPost("postPhase")]
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
    }
}

**/