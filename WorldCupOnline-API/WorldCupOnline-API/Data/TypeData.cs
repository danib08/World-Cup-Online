using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Bodies;
using Type = WorldCupOnline_API.Models.Type;

namespace WorldCupOnline_API.Data
{
    public class TypeData 
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all Types
        /// </summary>
        /// <returns>List of ValueIntBody object</returns>
        public async Task<List<ValueIntBody>> GetTypes() 
        {
            var list = new List<ValueIntBody>();///Create list of ValueIntBody object

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTypes", sql);///Calls stored procedure via sql connection
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
        /// Method to obtain one type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Type object</returns>
        public async Task<Type> GetOneType(int id)
        {
            var type = new Type();
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOneType", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    type = new Type
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"]
                    };
                }
            }
            return type; ///return type
        }

        /// <summary>
        /// Method to create Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task CreateType(Type type)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertType", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("id", type.id);
            cmd.Parameters.AddWithValue("name", type.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit types
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task EditType(int id, Type type)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editType", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", type.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete types
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteType(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteType", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("id", id); ///Add parameters with value

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}