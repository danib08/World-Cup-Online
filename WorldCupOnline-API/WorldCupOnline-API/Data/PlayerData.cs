using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;
using System.Numerics;

namespace WorldCupOnline_API.Data
{
    public class PlayerData
    {

        DbConection con = new DbConection();
        public async Task<List<Player>> GetPlayers()
        {
            var list = new List<Player>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("get_players", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var player = new Player();
                            player.id = (string)item["id"];
                            player.name = (string)item["name"];
                            player.lastname = (string)item["lastname"];
                            player.position = (string)item["position"];
                            list.Add(player);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Player>> GetOnePlayer(Player data)
        {
            var list = new List<Player>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOnePlayer", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var player = new Player();
                            player.id = (string)item["id"];
                            player.name = (string)item["name"];
                            player.lastname = (string)item["lastname"];
                            player.position = (string)item["position"];
                            list.Add(player);

                        }
                    }
                }

                return list;
            }
        }

        public async Task PostPlayer(Player player)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("insertPlayer", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", player.id);
                    cmd.Parameters.AddWithValue("@name", player.name);
                    cmd.Parameters.AddWithValue("@lastname", player.lastname);
                    cmd.Parameters.AddWithValue("@position", player.position);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task PutPlayer(Player player)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editPlayer", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", player.id);
                    cmd.Parameters.AddWithValue("@name", player.name);
                    cmd.Parameters.AddWithValue("@lastname", player.lastname);
                    cmd.Parameters.AddWithValue("@position", player.position);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }



        public async Task DeletePlayer(Player player)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_player", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", player.id);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }
    }
}
