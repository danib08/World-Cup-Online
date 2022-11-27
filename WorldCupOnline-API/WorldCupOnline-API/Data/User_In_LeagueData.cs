using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Controllers;

namespace WorldCupOnline_API.Data
{
    public class User_In_LeagueData
    {
        ///Create new connenction
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all User_In_League
        /// </summary>
        /// <returns>List of User_In_League objects</returns>
        public async Task<List<User_In_League>> GetUIL()
        {
            var list = new List<User_In_League>();///Creates a List of User_In_League objects
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getUIL", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    var uil = new User_In_League
                    {
                        id = (int)reader["id"],
                        leaegueid = (int)reader["leaegueid"],
                        userid = (string)reader["userid"]
                        
                    };
                    list.Add(uil);
                }
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain only one User_In_League
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User_In_League object</returns>
        public async Task<User_In_League> GetOneUIL(int id)
        {
            var uil = new User_In_League();///Creates object User_In_League
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getUIL", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    uil = new User_In_League
                    {
                        id = (int)reader["id"],
                        leaegueid = (int)reader["leaegueid"],
                        userid = (string)reader["userid"]
                    };
                }
            }
            return uil; ///Return object
        }

        /// <summary>
        /// Method to create a User_In_League
        /// </summary>
        /// <param name="uil"></param>
        /// <returns></returns>
        public async Task CreateUIL(User_In_League uil)
        {

            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertUIL", sql);///Calls stored procedure via sql connection

            cmd.CommandType = System.Data.CommandType.StoredProcedure; ///Indicates that command is a stored procedre
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@username", uil.userid);
            cmd.Parameters.AddWithValue("@accesscode", uil.leaegueid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();


        }

        /// <summary>
        /// Method to edit a User_In_League
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uil"></param>
        /// <returns></returns>
        public async Task EditLeague(int id, User_In_League uil)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editUIL", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@leaegueid", uil.leaegueid);
            cmd.Parameters.AddWithValue("@userid", uil.userid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete a User_In_League
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteUIL(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteUIL", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            cmd.Parameters.AddWithValue("@id", id);///Add parameter with value id

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

    }
}
