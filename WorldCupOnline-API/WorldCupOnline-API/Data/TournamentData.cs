using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Interfaces;

namespace WorldCupOnline_API.Data
{
    public class TournamentData : ITournamentData
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

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var tournament = new GetTournamentBody
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        startdate = (DateTime)reader["startdate"],
                        enddate = (DateTime)reader["enddate"],
                        description = (string)reader["description"],
                        type = (string)reader["type"]
                    };
                    list.Add(tournament); ///Add to list
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to get one tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>GetTournamentBody object</returns>
        public async Task<GetTournamentBody> GetOneTournament(string id)
        {
            var tournament = new GetTournamentBody(); ///Creation of GetTournamentBody object
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneTournament", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    tournament = new GetTournamentBody
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        startdate = (DateTime)reader["startdate"],
                        enddate = (DateTime)reader["enddate"],
                        description = (string)reader["description"],
                        type = (string)reader["type"]
                    };
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return tournament; ///Return object
        }

        /// <summary>
        /// Method to obtain all matches in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of MatchTournamentBody objects</returns>
        public async Task<List<MatchTournamentBody>> GetMatchesByTournament(string id)
        {
            var list = new List<MatchTournamentBody>(); ///MatchTournamentBody object creation
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getMatchesByTournament", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var tournamentMatches = new MatchTournamentBody
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        startdate = (DateTime)reader["startdate"],
                        starttime = (TimeSpan)reader["starttime"],
                        location = (string)reader["location"],
                        state = (string)reader["state"],
                        goalsteam1 = (int)reader["goalsteam1"],
                        goalsteam2 = (int)reader["goalsteam2"],
                    };
                    list.Add(tournamentMatches); ///Add to list
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain all phases in a tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of ValueIntBody</returns>
        public async Task<List<ValueIntBody>> GetPhasesByTournament(string id)
        {
            var list = new List<ValueIntBody>(); ///ValueIntBody list creation
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getPhasesByTournament", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
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
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to get teams by tournament
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of TeamTournamentBody object</returns>
        public async Task<List<TeamTournamentBody>> GetTeamsByTournament(string id)
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
                await reader.CloseAsync();
                await sql.CloseAsync();
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
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string newTournamentId = new(Enumerable.Repeat(chars, 6)
                                    .Select(s => s[random.Next(s.Length)]).ToArray());

            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTournament", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", newTournamentId);
            cmd.Parameters.AddWithValue("@name", tournament.name);
            cmd.Parameters.AddWithValue("@startdate", tournament.startdate);
            cmd.Parameters.AddWithValue("@enddate", tournament.enddate);
            cmd.Parameters.AddWithValue("@description", tournament.description);
            cmd.Parameters.AddWithValue("@typeid", tournament.typeid);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            await reader.CloseAsync();
            await sql.CloseAsync();

            ///Create teams in tournament for each team added
            foreach (string teamid in tournament.teamsIds)
            {
                var TIM = new Team_In_Tournament
                {
                    teamid = teamid,
                    tournamentid = newTournamentId
                };

                await CreateTeam_In_Tournament(TIM); ///Call method from other data file
            }

            ///Create phase in tournament for each team added
            foreach (string item in tournament.phases)
            {
                var phase = new Phase
                {
                    name = item,
                    tournamentID = newTournamentId
                };
                await CreatePhase(phase);///Call method from other data file
            }
        }

        /// <summary>
        /// Method to create Team_In_Tournament
        /// </summary>
        /// <param name="team_In_Tournament"></param>
        /// <returns></returns>
        public async Task CreateTeam_In_Tournament(Team_In_Tournament team_In_Tournament)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTIT", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                                                          ///Add parameters with value
            cmd.Parameters.AddWithValue("@teamid", team_In_Tournament.teamid);
            cmd.Parameters.AddWithValue("@tournamentid", team_In_Tournament.tournamentid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to create a phase
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        public async Task CreatePhase(Phase phase)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertPhase", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
                                                          ///Add parameters with value
            cmd.Parameters.AddWithValue("@name", phase.name);
            cmd.Parameters.AddWithValue("@tournamentid", phase.tournamentID);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
