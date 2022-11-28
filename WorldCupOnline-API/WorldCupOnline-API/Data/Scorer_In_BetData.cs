using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Scorer_In_BetData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to get all scorers in all bets
        /// </summary>
        /// <returns></returns>
        public async Task<List<Scorer_In_Bet>> GetScorer_In_Bet()
        {
            var list = new List<Scorer_In_Bet>(); ///Create new list of Scorers
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getSIB", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var scorer_In_Bet = new Scorer_In_Bet
                    {
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                    list.Add(scorer_In_Bet); ///Add to list
                }
            }
            return list; ///Returns list
        }

        /// <summary>
        /// Method to get one scorer in a bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ScorerInBer object</returns>
        public async Task<Scorer_In_Bet> GetOneScorer_In_Bet(int id)
        {
            var sib = new Scorer_In_Bet(); ///Create object scorerinbet
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneSIB", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);///Add parameter with value id

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    sib = new Scorer_In_Bet
                    {
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                }
            }
            return sib; ///return object
        }

        /// <summary>
        /// Method to create a scorer in a bet
        /// </summary>
        /// <param name="scorer_In_Bet"></param>
        /// <returns></returns>
        public async Task CreateScorer_In_Bet(Scorer_In_Bet scorer_In_Bet)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertSIB", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@betid", scorer_In_Bet.betid);
            cmd.Parameters.AddWithValue("@playerid", scorer_In_Bet.playerid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete Scorers in a Bet 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteScorer_In_Bet(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteSIB", sql);///Calls stored procedure via sql connection
            ///Add parameters with value
            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
