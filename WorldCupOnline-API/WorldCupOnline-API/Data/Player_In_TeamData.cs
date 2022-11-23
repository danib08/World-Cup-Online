using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Player_In_TeamData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to get all players in teams
        /// </summary>
        /// <returns></returns>
        public async Task <List<Player_In_Team>> GetPlayer_In_Team()
        {
            var list = new List<Player_In_Team>(); ///Create list of PlayerInteam object
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getPIT", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    var player_In_Team = new Player_In_Team
                    {
                        teamid = (string)reader["teamid"],
                        playerid = (string)reader["playerid"],
                        jerseynum = (int)reader["jerseynum"]
                    };
                    list.Add(player_In_Team);
                }
            }
            return list;
        }

        /// <summary>
        /// Method to obtain one player in one team
        /// </summary>
        /// <param name="teamid"></param>
        /// <param name="playerid"></param>
        /// <returns>Player_In_Team object</returns>
        public async Task<Player_In_Team> GetOnePlayer_In_Team(string teamid, string playerid)
        {
            var pit = new Player_In_Team();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOnePIT", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
                ///Add parameter with value
                cmd.Parameters.AddWithValue("@teamid", teamid);
                cmd.Parameters.AddWithValue("@playerid", playerid);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    var player_In_Team = new Player_In_Team
                    {
                        teamid = (string)reader["teamid"],
                        playerid = (string)reader["playerid"],
                        jerseynum = (int)reader["jerseynum"]
                    };
                }
            }
            return pit; ///return object
        }

        /// <summary>
        /// Method to craete Player in team
        /// </summary>
        /// <param name="player_In_Team"></param>
        /// <returns></returns>
        public async Task CreatePlayer_In_Team(Player_In_Team player_In_Team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertPIT", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            ///Add parameter with value 
            cmd.Parameters.AddWithValue("@teamid", player_In_Team.teamid);
            cmd.Parameters.AddWithValue("@playerid", player_In_Team.playerid);
            cmd.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit player in team
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        /// <param name="player_In_Team"></param>
        /// <returns></returns>
        public async Task EditPlayer_In_Team(string teamId, string playerId, Player_In_Team player_In_Team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editPIT", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            ///Add parameter with value 
            cmd.Parameters.AddWithValue("@teamid", teamId);
            cmd.Parameters.AddWithValue("@playerid", playerId);
            cmd.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete player in team
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task DeletePlayer_In_Team(string teamId, string playerId)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deletePIT", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            ///Add parameter with value 
            cmd.Parameters.AddWithValue("@teamid", teamId);
            cmd.Parameters.AddWithValue("@playerid", playerId);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
