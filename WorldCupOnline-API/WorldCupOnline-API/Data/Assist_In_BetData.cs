using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Assist_In_BetData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to get all Assist in Bet
        /// </summary>
        /// <returns>List of Assist in Bet object</returns>
        public async Task<List<Assist_In_Bet>> GetAssist_In_Bet()
        {
            var list = new List<Assist_In_Bet>(); ///Create list of Assist in bet
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getAIB", sql); ///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure; ///Indicates that command is a stored procedre

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var assist_In_Bet = new Assist_In_Bet ///Create assist in bet object
                    {
                        ///Read data from Database
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                    list.Add(assist_In_Bet); /// Add object to list
                }
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain an assist in bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Assist in Bet object</returns>
        public async Task<Assist_In_Bet> GetOneAssist_In_Bet(int id)
        {
            var aib = new Assist_In_Bet(); ///Create Assist in Bet object
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneAIB", sql)) ///Call stored procedure
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure; ///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    aib = new Assist_In_Bet 
                    {
                        ///Read data form Database
                        id = (int)reader["id"],
                        betid = (int)reader["betid"],
                        playerid = (string)reader["playerid"]
                    };
                }
            }
            return aib; ///Returns the object
        }

        /// <summary>
        /// Method to create assist in bet
        /// </summary>
        /// <param name="assist_In_Bet"></param>
        /// <returns></returns>
        public async Task CreateAssist_In_Bet(Assist_In_Bet assist_In_Bet)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertAIB", sql); ///Calls stored procedure

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@betid", assist_In_Bet.betid);
            cmd.Parameters.AddWithValue("@playerid", assist_In_Bet.playerid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete an assist in a bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAssist_In_Bet(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteAIB", sql); ///Calls the stored procedure from Database
            cmd.CommandType = CommandType.StoredProcedure; ///Indicates command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id);///Add parameter with value id

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
