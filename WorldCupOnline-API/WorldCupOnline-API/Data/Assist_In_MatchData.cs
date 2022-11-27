using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Assist_In_MatchData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to get all Assist in Match
        /// </summary>
        /// <returns>List of Assist in Match object</returns>
        public async Task<List<Assist_In_Match>> GetAssist_In_Match()
        {
            var list = new List<Assist_In_Match>(); ///Create list of Assist in Match
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getAIM", sql); ///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure; ///Indicates that command is a stored procedre

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var assist_In_Match = new Assist_In_Match ///Create assist in Match object
                    {
                        ///Read data from Database
                        id = (int)reader["id"],
                        matchid = (int)reader["matchid"],
                        playerid = (string)reader["playerid"]
                    };
                    list.Add(assist_In_Match); /// Add object to list
                }
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain an assist in Match
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Assist in Match object</returns>
        public async Task<Assist_In_Match> GetOneAssist_In_Match(int id)
        {
            var aim = new Assist_In_Match(); ///Create Assist in Match object
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneAIM", sql)) ///Call stored procedure
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure; ///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    aim = new Assist_In_Match 
                    {
                        ///Read data form Database
                        id = (int)reader["id"],
                        matchid = (int)reader["matchid"],
                        playerid = (string)reader["playerid"]
                    };
                }
            }
            return aim; ///Returns the object
        }

        /// <summary>
        /// Method to create assist in Match
        /// </summary>
        /// <param name="assist_In_Match"></param>
        /// <returns></returns>
        public async Task CreateAssist_In_Match(Assist_In_Match assist_In_Match)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertAIM", sql); ///Calls stored procedure

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@matchid", assist_In_Match.matchid);
            cmd.Parameters.AddWithValue("@playerid", assist_In_Match.playerid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete an assist in a Match
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAssist_In_Match(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteAIM", sql); ///Calls the stored procedure from Database
            cmd.CommandType = CommandType.StoredProcedure; ///Indicates command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id);///Add parameter with value id

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
