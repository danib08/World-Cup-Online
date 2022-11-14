using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Player_In_TeamData
    {
        private readonly DbConection _con = new();
        public async Task <List<Player_In_Team>> GetPlayer_In_Team()
        {
            var list = new List<Player_In_Team>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getPIT", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
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

        public async Task<List<Player_In_Team>> GetOnePlayer_In_Team(string teamid, string playerid)
        {
            var list = new List<Player_In_Team>();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOnePIT", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@teamid", teamid);
                cmd.Parameters.AddWithValue("@playerid", playerid);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
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

        public async Task CreatePlayer_In_Team(Player_In_Team player_In_Team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertPIT", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamid", player_In_Team.teamid);
            cmd.Parameters.AddWithValue("@playerid", player_In_Team.playerid);
            cmd.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task EditPlayer_In_Team(string teamId, string playerId, Player_In_Team player_In_Team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editPIT", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamid", teamId);
            cmd.Parameters.AddWithValue("@playerid", playerId);
            cmd.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeletePlayer_In_Team(string teamId, string playerId)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("delete_PIT", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamid", teamId);
            cmd.Parameters.AddWithValue("@playerid", playerId);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
