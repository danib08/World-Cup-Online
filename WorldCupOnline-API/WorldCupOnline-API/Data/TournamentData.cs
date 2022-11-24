using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Data
{
    public class TournamentData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all tournaments
        /// </summary>
        /// <returns>List of GetTournamentBody object</returns>
        public async Task<List<GetTournamentBody>> GetTournament()
        {
            var list = new List<GetTournamentBody>(); //Create GetTournamentBody object
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTournaments", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    ///Read from database
                    var tournament = new GetTournamentBody
                    {
                        id = (int)item["id"],
                        name = (string)item["name"],
                        startdate = (DateTime)item["startdate"],
                        enddate = (DateTime)item["enddate"],
                        description = (string)item["description"],
                        type = (string)item["type"]
                    };
                    list.Add(tournament); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to get one tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>GetTournamentBody object</returns>
        public async Task<GetTournamentBody> GetOneTournament(int id)
        {
            var tournament = new GetTournamentBody(); ///Creation of GetTournamentBody object
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneTournament", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    ///Read from database
                    tournament = new GetTournamentBody
                    {
                        id = (int)item["id"],
                        name = (string)item["name"],
                        startdate = (DateTime)item["startdate"],
                        enddate = (DateTime)item["enddate"],
                        description = (string)item["description"],
                        type = (string)item["type"]
                    };
                }
            }
            return tournament; ///Return object
        }

        /// <summary>
        /// Method to obtain all matches in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of MatchTournamentBody objects</returns>
        public async Task<List<MatchTournamentBody>> GetMatchesByTournament(int id)
        {
            var list = new List<MatchTournamentBody>(); ///MatchTournamentBody object creation
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getMatchesByTournament", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    ///Read from database
                    var tournamentMatches = new MatchTournamentBody
                    {
                        id = (int)item["id"],
                        name = (string)item["name"],
                        startdate = (DateTime)item["startdate"],
                        starttime = (TimeSpan)item["starttime"],
                        location = (string)item["location"],
                        state = (string)item["state"],
                        score = (string)item["score"]
                    };
                    list.Add(tournamentMatches); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain all phases in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of ValueIntBody</returns>
        public async Task<List<ValueIntBody>> GetPhasesByTournament(int id)
        {
            var list = new List<ValueIntBody>(); ///ValueIntBody list creation
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getPhasesByTournament", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var item = new ValueIntBody
                    {
                        value = (int)reader["value"],
                        label = (string)reader["label"]
                    };
                    list.Add(item);///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to get teams by tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of TeamTournamentBody object</returns>
        public async Task<List<TeamTournamentBody>> GetTeamsByTournament(int id)
        {
            var list = new List<TeamTournamentBody>(); ///TeamTournamentBody list creation
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getTeamsByTournament", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var team = new TeamTournamentBody
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        confederation = (string)reader["confederation"]
                    };
                    list.Add(team); ///Add to list
                }
            }
            return list; /// Return list
        }


        /// <summary>
        /// Method to create tournament
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns></returns>
        public async Task CreateTournament(TournamentCreator tournament)
        {
            int newTournamentId = 0;
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTournament", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@name", tournament.name);
            cmd.Parameters.AddWithValue("@startdate", tournament.startdate);
            cmd.Parameters.AddWithValue("@enddate", tournament.enddate);
            cmd.Parameters.AddWithValue("@description", tournament.description);
            cmd.Parameters.AddWithValue("@typeid", tournament.typeid);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader["id"];
                newTournamentId = Convert.ToInt32(id);
            }

            await reader.CloseAsync();
            await sql.CloseAsync();

            ///Create teams in tournament for each team added
            var teams = new Team_In_TournamentData();
            foreach (string teamid in tournament.teamsIds)
            {
                var TIM = new Team_In_Tournament
                {
                    teamid = teamid,
                    tournamentid = newTournamentId
                };

                await teams.CreateTeam_In_Tournament(TIM); ///Call method from other data file
            }

            ///Create phase in tournament for each team added
            var phases = new PhaseData();
            foreach (string item in tournament.phases)
            {
                var phase = new Phase
                {
                    name = item,
                    tournamentID = newTournamentId
                };
                await phases.CreatePhase(phase);///Call method from other data file
            }
        }

        /// <summary>
        /// Method to edit tournaments
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tournament"></param>
        /// <returns></returns>
        public async Task EditTournament(int id, Tournament tournament)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editTournament", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", tournament.name);
            cmd.Parameters.AddWithValue("@startdate", tournament.startdate);
            cmd.Parameters.AddWithValue("@enddate", tournament.enddate);
            cmd.Parameters.AddWithValue("@description", tournament.description);
            cmd.Parameters.AddWithValue("@typeid", tournament.typeid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteTournament(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteTournament", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id); ///Add parameter with value

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
