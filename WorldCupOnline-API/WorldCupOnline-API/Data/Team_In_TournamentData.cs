using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Team_In_TournamentData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all Team_In_Tournament
        /// </summary>
        /// <returns>List of Team_In_Tournament</returns>
        public async Task<List<Team_In_Tournament>> GetTeam_In_Tournament()
        {
            var list = new List<Team_In_Tournament>();///Create list of Team_In_Tournament object
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTIT", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var team_In_Tournament = new Team_In_Tournament
                    {
                        teamid = (string)reader["teamid"],
                        tournamentid = (int)reader["tournamentid"]
                    };
                    list.Add(team_In_Tournament);///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain one Team_In_Tournament
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="tournamentid"></param>
        /// <returns>Team_In_Tournament object</returns>
        public async Task<Team_In_Tournament> GetOneTeam_In_Tournament(string teamid, int tournamentid)
        {
            var tit = new Team_In_Tournament(); ///Create new Team_In_Tournament object
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneTIT", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                ///Add parameters with value
                cmd.Parameters.AddWithValue("@teamid", teamid);
                cmd.Parameters.AddWithValue("@tournamentid", tournamentid);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    tit = new Team_In_Tournament
                    {
                        teamid = (string)reader["teamid"],
                        tournamentid = (int)reader["tournamentid"]
                    };
                }
            }
            return tit; ///Return object
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

            cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@teamid", team_In_Tournament.teamid);
            cmd.Parameters.AddWithValue("@tournamentid", team_In_Tournament.tournamentid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete Team_In_Tournament
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="tournamentid"></param>
        /// <returns></returns>
        public async Task DeleteTeam_In_Tournament(string teamid, int tournamentid)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteTIT", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@teamid", teamid);
            cmd.Parameters.AddWithValue("@tournamentid", tournamentid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
