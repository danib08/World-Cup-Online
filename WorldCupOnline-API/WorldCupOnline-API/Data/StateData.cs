using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class StateData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to create States
        /// </summary>
        /// <returns>List of State objects</returns>
        public async Task<List<State>> GetStates()
        {
            var list = new List<State>(); ///Create List of States

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getStates", sql); ///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var state = new State
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"]
                    };
                    list.Add(state); ///Add to list
                }
            }
            return list;///Return list
        }

        /// <summary>
        /// Method to get one state
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Object State</returns>
        public async Task<State> GetOneState(int id)
        {
            var state = new State(); ///Create object State
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOneState", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    state = new State
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"]
                    };
                }
            }
            return state; ///Return object
        }

        /// <summary>
        /// Method to Create State
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task CreateState(State state)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertState", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", state.id);
            cmd.Parameters.AddWithValue("@name", state.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit States
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task EditState(int id, State state)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editState", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", state.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete a State
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteState(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteState", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value id

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
