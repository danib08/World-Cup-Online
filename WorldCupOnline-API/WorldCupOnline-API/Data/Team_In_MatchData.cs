using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Team_In_MatchData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all team in all matches
        /// </summary>
        /// <returns></returns>
        public async Task<List<Team_In_Match>> GetTeam_In_Match()
        {
            var list = new List<Team_In_Match>();///Create list of Team_In_Match object
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTIM", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var team_In_Match = new Team_In_Match
                    {
                        teamid = (string)reader["teamid"],
                        matchid = (int)reader["matchid"]
                    };
                    list.Add(team_In_Match); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain one team in a match
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="matchid"></param>
        /// <returns>Team_In_Match object</returns>
        public async Task<Team_In_Match> GetOneTeam_In_Match(string teamid, int matchid)
        {
            var tim = new Team_In_Match(); ///Create object Team_In_Match
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneTIM", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@teamid", teamid);
                cmd.Parameters.AddWithValue("@matchid", matchid);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    tim = new Team_In_Match
                    {
                        teamid = (string)reader["teamid"],
                        matchid = (int)reader["matchid"]
                    };
                }
            }
            return tim; ///Return object
        }

        /// <summary>
        /// Method to create Team_In_Match
        /// </summary>
        /// <param name="team_In_Match"></param>
        /// <returns></returns>
        public async Task CreateTeam_In_Match(Team_In_Match team_In_Match)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTIM", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@teamid", team_In_Match.teamid);
            cmd.Parameters.AddWithValue("@matchid", team_In_Match.matchid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete Team_In_Match
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public async Task DeleteTeam_In_Match(string teamid, int matchid)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteTIM", sql);///Calls stored procedure via sql connection
            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@teamid", teamid);
            cmd.Parameters.AddWithValue("@matchid", matchid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
