using System.Collections.Generic;
using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;


namespace WorldCupOnline_API.Data{
    public class TypeData{
    DbConection con = new DbConection();
        public async Task <List<Type>> GetTypes()
        {
            var list = new List<Type>();
            using(var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("get_types",sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using(var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var type = new Type();
                            type.id = (int)item["id"];
                            type.name = (string)item["name"];
                            list.Add(type);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Type>> GetOneType(Type data)
        {
            var list = new List<Type>();
            using(var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cms = new SqlCommand("getOneType",sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id",data.id);

                    using(var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var type = new Type();
                            type.id = (int)item["id"];
                            type.name = (string)item["name"];
                            list.Add(type);
                        }
                    }
                }
                return list;
            }
        }

        public async Task PostType(Type type)
        {
            using(var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("insertType",sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", type.id);
                    cmd.Parameters.AddWithValue("name", type.name);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();
                }
            }
        }

        public async Task PutType(Type type)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("editType",sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", type.id);
                    cmd.Parameters.AddWithValue("name". type.name);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();
                }
            }
        }

        public async Task DeleteType(Type type)
        {
            using(var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("delete_type",sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", type.id);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();
                }
            }
        }
    }
}