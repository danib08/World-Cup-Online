using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class StateData
    {

        DbConection con = new DbConection();
        public async Task<List<State>> GetStates()
        {
            var list = new List<State>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("get_state", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var state = new State();
                            state.id = (int)item["id"];
                            state.name = (string)item["name"];
                            list.Add(state);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<State>> GetOneState(State data)
        {
            var list = new List<State>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOneState", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var state = new State();
                            state.id = (int)item["id"];
                            state.name = (string)item["name"];
                            list.Add(state);

                        }
                    }
                }

                return list;
            }
        }


        public async Task PostState(State state)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("insertState", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", state.id);
                    cmd.Parameters.AddWithValue("@name", state.name);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task PutState(State state)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editState", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", state.id);
                    cmd.Parameters.AddWithValue("@name", state.name);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task DeleteState(State state)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_state", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", state.id);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }
    }
}
