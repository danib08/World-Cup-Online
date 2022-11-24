using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class MatchData
    {
        ///Create new connenction
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all matches
        /// </summary>
        /// <returns>List of Match objects</returns>
        public async Task <List<Match>> GetMatches()
        {
            var list = new List<Match>();///Creates a List of Match objects
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getMatches", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    var match = new Match
                    {
                        id = (int)reader["id"],
                        startdate = (DateTime)reader["startdate"],
                        starttime = (TimeSpan)reader["starttime"],
                        goalsteam1 = (int)reader["goalsteam1"],
                        goalsteam2 = (int)reader["goalsteam2"],
                        location = (string)reader["location"],
                        stateid = (int)reader["stateid"],
                        tournamentid = (int)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"],
                        mvp = (string)reader["mvp"]
                    };
                    list.Add(match);
                }
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain only one Match
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Match object</returns>
        public async Task<Match> GetOneMatch(int id)
        {
            var match = new Match();///Creates object Match
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneMatch", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    match = new Match
                    {
                        id = (int)reader["id"],
                        startdate = (DateTime)reader["startdate"],
                        starttime = (TimeSpan)reader["starttime"],
                        goalsteam1 = (int)reader["goalsteam1"],
                        goalsteam2 = (int)reader["goalsteam2"],
                        location = (string)reader["location"],
                        stateid = (int)reader["stateid"],
                        tournamentid = (int)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"],
                        mvp = (string)reader["mvp"]
                    };
                }
            }
            return match; ///Return object
        }

        /// <summary>
        /// Method to create a match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public async Task CreateMatch(MatchCreator match)
        {
            int newMatchId = 0;
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertMatch", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@startdate", match.startdate);
            cmd.Parameters.AddWithValue("@starttime", match.starttime);
            cmd.Parameters.AddWithValue("@goalsteam1", 0);
            cmd.Parameters.AddWithValue("@goalsteam2", 0);
            cmd.Parameters.AddWithValue("@location", match.location);
            cmd.Parameters.AddWithValue("@stateid", 1);
            cmd.Parameters.AddWithValue("@tournamentid", match.tournamentid);
            cmd.Parameters.AddWithValue("@phaseid", match.phaseid);
            cmd.Parameters.AddWithValue("@mvp", match.mvpid);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader["id"];
                newMatchId = Convert.ToInt32(id);
            }

            await reader.CloseAsync();
            await sql.CloseAsync();
            var teams = new Team_In_MatchData();

            ///Create team1 in match when creating a match
            var team1 = new Team_In_Match
            {
                teamid = match.team1,
                matchid = newMatchId
            };

            ///Create team2 in match when creating a match
            var team2 = new Team_In_Match
            {
                teamid = match.team2,
                matchid = newMatchId
            };

            ///Executing methods to create from other data files
            await teams.CreateTeam_In_Match(team1);
            await teams.CreateTeam_In_Match(team2);
        }

        /// <summary>
        /// Method to edit a match
        /// </summary>
        /// <param name="id"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public async Task EditMatch(int id, BetCreator match)
        {
            
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("updateMatch", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@goalsteam1", match.team1goals);
            cmd.Parameters.AddWithValue("@goalsteam2", match.team2goals);
            cmd.Parameters.AddWithValue("@mvp", match.mvpid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();

            var scorers = new Scorer_In_MatchData();
            var assists = new Assist_In_MatchData();

            foreach(string scorerId in match.team1scorers)
            {
                var SIM = new Scorer_In_Match
                {
                    matchid = id,
                    playerid = scorerId

                };
                await scorers.CreateScorer_In_Match(SIM);
            }

            foreach (string scorerId in match.team2scorers)
            {
                var SIM = new Scorer_In_Match
                {
                    matchid = id,
                    playerid = scorerId

                };
                await scorers.CreateScorer_In_Match(SIM);
            }

            foreach(string assistId in match.team1assists)
            {
                var AIM = new Assist_In_Match
                {
                    matchid = id,
                    playerid = assistId
                };
                await assists.CreateAssist_In_Match(AIM);
            }

            foreach (string assistId in match.team2assists)
            {
                var AIM = new Assist_In_Match
                {
                    matchid = id,
                    playerid = assistId
                };
                await assists.CreateAssist_In_Match(AIM);
            }

        }

        /// <summary>
        /// Method to delete a match
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteMatch(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteMatch", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            cmd.Parameters.AddWithValue("@id", id);///Add parameter with value id

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

    }
}
