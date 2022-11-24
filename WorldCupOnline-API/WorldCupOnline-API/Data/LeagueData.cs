using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class LeagueData
    {
        ///Create new connenction
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all Leagues
        /// </summary>
        /// <returns>List of League objects</returns>
        public async Task <List<League>> GetLeague()
        {
            var list = new List<League>();///Creates a List of League objects
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getLeague", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    var league = new League
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        accesscode = (int)reader["accesscode"],
                        tournamentid = (int)reader["tournamentid"],
                        userid = (string)reader["userid"]
                    };
                    list.Add(league);
                }
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain only one League
        /// </summary>
        /// <param name="id"></param>
        /// <returns>League object</returns>
        public async Task<League> GetOneLeague(int id)
        {
            var league = new League();///Creates object League
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneLeague", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    league = new League
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        accesscode = (int)reader["accesscode"],
                        tournamentid = (int)reader["tournamentid"],
                        userid = (string)reader["userid"]
                    };
                }
            }
            return league; ///Return object
        }

        /// <summary>
        /// Method to obtain all Types
        /// </summary>
        /// <returns>List of ValueIntBody object</returns>
        public async Task<List<ValueIntBody>> GetTournaments()
        {
            var list = new List<ValueIntBody>();///Create list of ValueIntBody object

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTournLeagues", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var type = new ValueIntBody
                    {
                        value = (int)reader["value"],
                        label = (string)reader["label"]
                    };
                    list.Add(type); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to create League
        /// </summary>
        /// <param name="league"></param>
        /// <returns></returns>
        public async Task CreateLeague(LeagueCreator league)
        {
            Random random = new Random();
            int randomNumber = random.Next();
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertLeague", sql);///Calls stored procedure via sql connection

            cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@name", league.name);
            cmd.Parameters.AddWithValue("@accesscode", Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));
            cmd.Parameters.AddWithValue("@tournamentid", league.tournamentid);
            cmd.Parameters.AddWithValue("@userid", league.userid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit a league
        /// </summary>
        /// <param name="id"></param>
        /// <param name="league"></param>
        /// <returns></returns>
        public async Task EditLeague(int id, League league)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editLeague", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", league.id);
            cmd.Parameters.AddWithValue("@name", league.name);
            cmd.Parameters.AddWithValue("@accesscode", league.accesscode);
            cmd.Parameters.AddWithValue("@tournamentid", league.tournamentid);
            cmd.Parameters.AddWithValue("@userid", league.userid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete a league
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteLeague(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteLeague", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            cmd.Parameters.AddWithValue("@id", id);///Add parameter with value id

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

    }
}
