using System.Collections.Generic;
using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Player_In_TeamData
    {
        DbConection con = new DbConection();
        public async Task <List<Player_In_Team>> GetPlayer_In_Team()
        {
            var list = new List<Player_In_Team>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("get_PIT", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using(var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var player_In_Team = new Player_In_Team();
                            player_In_Team.teamid = (string)item["teamid"];
                            player_In_Team.playerid = (string)item["playerid"];
                            player_In_Team.jerseynum = (int)item["jerseynum"];
                            list.Add(player_In_Team);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Player_In_Team>> GetOnePlayer_In_Team(Player_In_Team data)
        {
            var list = new List<Player_In_Team>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOnePIT", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var player_In_Team = new Player_In_Team();
                            player_In_Team.teamid = (string)item["teamid"];
                            player_In_Team.playerid = (string)item["playerid"];
                            player_In_Team.jerseynum = (int)item["jerseynum"];
                            list.Add(player_In_Team);

                        }
                    }
                }

                return list;
            }
        }

        public async Task PostPlayer_In_Team(Player_In_Team player_In_Team)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("insertPIT", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", player_In_Team.teamid);
                    cmd.Parameters.AddWithValue("@playerid", player_In_Team.playerid);
                    cmd.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task PutPlayer_In_Team(Player_In_Team player_In_Team)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editPIT", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", player_In_Team.teamid);
                    cmd.Parameters.AddWithValue("@playerid", player_In_Team.playerid);
                    cmd.Parameters.AddWithValue("@jerseynum", player_In_Team.jerseynum);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task DeletePlayer_In_Team(Player_In_Team player_In_Team)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_PIT", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", player_In_Team.teamid);
                    cmd.Parameters.AddWithValue("@playerid", player_In_Team.playerid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

    }
}
