using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Interfaces;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class MatchData : IMatchData
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
                        tournamentid = (string)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"],
                        mvp = (string)reader["mvp"]
                    };
                    list.Add(match);
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
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
                        tournamentid = (string)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"],
                        mvp = (string)reader["mvp"]
                    };
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
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
            cmd.Parameters.AddWithValue("@mvp", "mvp");

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader["id"];
                newMatchId = Convert.ToInt32(id);
            }
            await reader.CloseAsync();

            //Create Team_In_Match
            using var cmdTeam1 = new SqlCommand("insertTIM", sql);
            cmdTeam1.CommandType = CommandType.StoredProcedure;
            cmdTeam1.Parameters.AddWithValue("@teamid", match.team1);
            cmdTeam1.Parameters.AddWithValue("@matchid", newMatchId);

            using var readerTeam1 = await cmdTeam1.ExecuteReaderAsync();
            await readerTeam1.CloseAsync();

            //Create Team_In_Match
            using var cmdTeam2 = new SqlCommand("insertTIM", sql);
            cmdTeam2.CommandType = CommandType.StoredProcedure;
            cmdTeam2.Parameters.AddWithValue("@teamid", match.team2);
            cmdTeam2.Parameters.AddWithValue("@matchid", newMatchId);

            using var readerTeam2 = await cmdTeam2.ExecuteReaderAsync();
            await readerTeam2.CloseAsync();

            await sql.CloseAsync();
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
            using var reader = await cmd.ExecuteReaderAsync();
            await reader.CloseAsync();

            //Create Scorer_In_Match for Team 1
            foreach (string scorerId in match.team1scorers)
            {
                using var cmdScorer1 = new SqlCommand("insertSIM", sql);
                cmdScorer1.CommandType = CommandType.StoredProcedure;
                cmdScorer1.Parameters.AddWithValue("@matchid", id);
                cmdScorer1.Parameters.AddWithValue("@playerid", scorerId);

                using var readerScorer1 = await cmdScorer1.ExecuteReaderAsync();
                await readerScorer1.CloseAsync();
            }

            //Create Scorer_In_Match for Team 2
            foreach (string scorerId in match.team2scorers)
            {
                using var cmdScorer2 = new SqlCommand("insertSIM", sql);
                cmdScorer2.CommandType = CommandType.StoredProcedure;
                cmdScorer2.Parameters.AddWithValue("@matchid", id);
                cmdScorer2.Parameters.AddWithValue("@playerid", scorerId);

                using var readerScorer1 = await cmdScorer2.ExecuteReaderAsync();
                await readerScorer1.CloseAsync();
            }

            //Create Assist_In_Match for Team 1
            foreach (string assistId in match.team1assists)
            {
                using var cmdAssist1 = new SqlCommand("insertAIM", sql);
                cmdAssist1.CommandType = CommandType.StoredProcedure;
                cmdAssist1.Parameters.AddWithValue("@matchid", id);
                cmdAssist1.Parameters.AddWithValue("@playerid", assistId);

                using var readerAssist1 = await cmdAssist1.ExecuteReaderAsync();
                await readerAssist1.CloseAsync();
             
            }

            //Create Assist_In_Match for Team 2
            foreach (string assistId in match.team2assists)
            {
                using var cmdAssist2 = new SqlCommand("insertAIM", sql);
                cmdAssist2.CommandType = CommandType.StoredProcedure;
                cmdAssist2.Parameters.AddWithValue("@matchid", id);
                cmdAssist2.Parameters.AddWithValue("@playerid", assistId);

                using var readerAssist2 = await cmdAssist2.ExecuteReaderAsync();
                await readerAssist2.CloseAsync();
            }
        }
    }
}
