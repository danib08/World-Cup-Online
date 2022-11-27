using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Scorer_In_MatchData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to get all scorers in all Matches
        /// </summary>
        /// <returns></returns>
        public async Task<List<Scorer_In_Match>> GetScorer_In_Match()
        {
            var list = new List<Scorer_In_Match>(); ///Create new list of Scorers
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getSIM", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var scorer_In_Match = new Scorer_In_Match
                    {
                        id = (int)reader["id"],
                        matchid = (int)reader["matchid"],
                        playerid = (string)reader["playerid"]
                    };
                    list.Add(scorer_In_Match); ///Add to list
                }
            }
            return list; ///Returns list
        }

        /// <summary>
        /// Method to get one scorer in a Match
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ScorerInMatch object</returns>
        public async Task<Scorer_In_Match> GetOneScorer_In_Match(int id)
        {
            var sim = new Scorer_In_Match(); ///Create object scorerinMatch
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneSIM", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);///Add parameter with value id

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    sim = new Scorer_In_Match
                    {
                        id = (int)reader["id"],
                        matchid = (int)reader["matchid"],
                        playerid = (string)reader["playerid"]
                    };
                }
            }
            return sim; ///return object
        }

        /// <summary>
        /// Method to create a scorer in a Match
        /// </summary>
        /// <param name="scorer_In_Match"></param>
        /// <returns></returns>
        public async Task CreateScorer_In_Match(Scorer_In_Match scorer_In_Match)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertSIM", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@matchid", scorer_In_Match.matchid);
            cmd.Parameters.AddWithValue("@playerid", scorer_In_Match.playerid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete Scorers in a Match 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteScorer_In_Match(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteSIM", sql);///Calls stored procedure via sql connection
            ///Add parameters with value
            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
