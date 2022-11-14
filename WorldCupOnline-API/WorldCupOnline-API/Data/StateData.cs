using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class StateData
    {
        private readonly DbConection _con = new();

        public async Task<List<State>> GetStates()
        {
            var list = new List<State>();

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getStates", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var state = new State
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"]
                    };
                    list.Add(state);
                }
            }
            return list;
        }

        public async Task<List<State>> GetOneState(int id)
        {
            var list = new List<State>();
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOneState", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var state = new State
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"]
                    };
                    list.Add(state);
                }
            }
            return list;
        }

        public async Task CreateState(State state)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertState", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", state.id);
            cmd.Parameters.AddWithValue("@name", state.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task EditState(int id, State state)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editState", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", state.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteState(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("delete_state", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
