using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Scorer_In_BetData
    {
        private readonly DbConnection _con = new();

        public async Task<List<Scorer_In_Bet>> GetScorer_In_Bet()
        {
            var list = new List<Scorer_In_Bet>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getSIB", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var scorer_In_Bet = new Scorer_In_Bet
                    {
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                    list.Add(scorer_In_Bet);
                }
            }
            return list;
        }

        public async Task<Scorer_In_Bet> GetOneScorer_In_Bet(int id)
        {
            var sib = new Scorer_In_Bet();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneSIB", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    sib = new Scorer_In_Bet
                    {
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                }
            }
            return sib;
        }

        public async Task CreateScorer_In_Bet(Scorer_In_Bet scorer_In_Bet)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertSIB", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@betid", scorer_In_Bet.betid);
            cmd.Parameters.AddWithValue("@playerid", scorer_In_Bet.playerid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteScorer_In_Bet(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteSIB", sql);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
