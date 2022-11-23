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
                        score = (string)reader["score"],
                        location = (string)reader["location"],
                        stateid = (int)reader["stateid"],
                        tournamentid = (int)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"]
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
                        score = (string)reader["score"],
                        location = (string)reader["location"],
                        stateid = (int)reader["stateid"],
                        tournamentid = (int)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"]
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
            cmd.Parameters.AddWithValue("@score", "0-0");
            cmd.Parameters.AddWithValue("@location", match.location);
            cmd.Parameters.AddWithValue("@stateid", 1);
            cmd.Parameters.AddWithValue("@tournamentid", match.tournamentid);
            cmd.Parameters.AddWithValue("@phaseid", match.phaseid);

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
        public async Task EditMatch(int id, Match match)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editMatch", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

            ///Add parameters with valuecmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@startdate", match.startdate);
            cmd.Parameters.AddWithValue("@starttime", match.starttime);
            cmd.Parameters.AddWithValue("@score", match.score);
            cmd.Parameters.AddWithValue("@location", match.location);
            cmd.Parameters.AddWithValue("@stateid", match.stateid);
            cmd.Parameters.AddWithValue("@tournamentid", match.tournamentid);
            cmd.Parameters.AddWithValue("@phaseid", match.phaseid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
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
