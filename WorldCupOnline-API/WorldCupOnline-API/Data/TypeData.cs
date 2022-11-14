using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Bodies;
using Type = WorldCupOnline_API.Models.Type;

namespace WorldCupOnline_API.Data
{
    public class TypeData 
    {
        private readonly DbConection _con = new();

        public async Task <List<ValueIntBody>> GetTypes()
        {
            var list = new List<ValueIntBody>();

            using(var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTypes", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var type = new ValueIntBody
                    {
                        value = (int)reader["value"],
                        label = (string)reader["label"]
                    };
                    list.Add(type);
                }
            }
            return list;
        }

        public async Task<List<Type>> GetOneType(int id)
        {
            var list = new List<Type>();
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOneType", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var type = new Type
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"]
                    };
                    list.Add(type);
                }
            }
            return list;
        }

        public async Task CreateType(Type type)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertType", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", type.id);
            cmd.Parameters.AddWithValue("name", type.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task EditType(int id, Type type)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editType", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", type.name);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteType(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteType", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}