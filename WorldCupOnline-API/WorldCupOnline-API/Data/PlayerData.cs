using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class PlayerData
    {
        private readonly DbConnection _con = new();

        public async Task<List<Player>> GetPlayers()
        {
            var list = new List<Player>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getPlayers", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    var player = new Player
                    {
                        id = (string)item["id"],
                        name = (string)item["name"],
                        lastname = (string)item["lastname"],
                        position = (string)item["position"]
                    };
                    list.Add(player);
                }
            }
            return list;
        }

        public async Task<Player> GetOnePlayer(string id)
        {
            var player = new Player();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOnePlayer", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    player = new Player
                    {
                        id = (string)item["id"],
                        name = (string)item["name"],
                        lastname = (string)item["lastname"],
                        position = (string)item["position"]
                    };
                }
            }
            return player;
        }

        public async Task CreatePlayer(Player player)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertPlayer", sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", player.id);
            cmd.Parameters.AddWithValue("@name", player.name);
            cmd.Parameters.AddWithValue("@lastname", player.lastname);
            cmd.Parameters.AddWithValue("@position", player.position);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task EditPlayer(string id, Player player)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editPlayer", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", player.name);
            cmd.Parameters.AddWithValue("@lastname", player.lastname);
            cmd.Parameters.AddWithValue("@position", player.position);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeletePlayer(string id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("delete_player", sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
