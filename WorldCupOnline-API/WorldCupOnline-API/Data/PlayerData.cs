using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class PlayerData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all Players
        /// </summary>
        /// <returns>List of Player object</returns>
        public async Task<List<Player>> GetPlayers()
        {
            var list = new List<Player>(); ///Creat list of players
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getPlayers", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    ///Read from database
                    var player = new Player
                    {
                        id = (string)item["id"],
                        name = (string)item["name"],
                        lastname = (string)item["lastname"],
                        position = (string)item["position"]
                    };
                    list.Add(player); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to get one Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Player object</returns>
        public async Task<Player> GetOnePlayer(string id)
        {
            var player = new Player(); ///Create new object Player
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOnePlayer", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    ///Read from database
                    player = new Player
                    {
                        id = (string)item["id"],
                        name = (string)item["name"],
                        lastname = (string)item["lastname"],
                        position = (string)item["position"]
                    };
                }
            }
            return player; ///Return object
        }

        /// <summary>
        /// Method to create Players
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public async Task CreatePlayer(Player player)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertPlayer", sql);///Calls stored procedure via sql connection

            cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", player.id);
            cmd.Parameters.AddWithValue("@name", player.name);
            cmd.Parameters.AddWithValue("@lastname", player.lastname);
            cmd.Parameters.AddWithValue("@position", player.position);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit Players
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public async Task EditPlayer(string id, Player player)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editPlayer", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", player.name);
            cmd.Parameters.AddWithValue("@lastname", player.lastname);
            cmd.Parameters.AddWithValue("@position", player.position);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// method to delete player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePlayer(string id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deletePlayer", sql);///Calls stored procedure via sql connection

            cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id);///Add parameter with value

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
