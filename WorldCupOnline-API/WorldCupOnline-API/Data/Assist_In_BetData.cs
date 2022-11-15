using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Assist_In_BetData
    {
        private readonly DbConnection _con = new();

        public async Task<List<Assist_In_Bet>> GetAssist_In_Bet()
        {
            var list = new List<Assist_In_Bet>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getAIB", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var assist_In_Bet = new Assist_In_Bet
                    {
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                    list.Add(assist_In_Bet);
                }
            }
            return list;
        }

        public async Task<Assist_In_Bet> GetOneAssist_In_Bet(int id)
        {
            var aib = new Assist_In_Bet();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneAIB", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    aib = new Assist_In_Bet
                    {
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                }
            }
            return aib;
        }

        public async Task CreateAssist_In_Bet(Assist_In_Bet assist_In_Bet)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertAIB", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@betid", assist_In_Bet.betid);
            cmd.Parameters.AddWithValue("@playerid", assist_In_Bet.playerid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteAssist_In_Bet(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteAIB", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
