using System.Collections.Generic;
using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;


namespace WorldCupOnline_API.Data{
    public class TypeData{
    DbConection con = new DbConection();
        public async Task <List<Models.Type>> GetTypes()
        {
            var list = new List<Models.Type>();
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
                            var type = new Models.Type();
                            type.id = (int)item["id"];
                            type.name = (string)item["name"];
                            list.Add(type);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Models.Type>> GetOneType(Models.Type data)
        {
            var list = new List<Models.Type>();
            using(var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("getOneType",sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id",data.id);

                    using(var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var type = new Models.Type();
                            type.id = (int)item["id"];
                            type.name = (string)item["name"];
                            list.Add(type);
                        }
                    }
                }
                return list;
            }
        }

        public async Task PostType(Models.Type type)
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

        public async Task PutType(Models.Type type)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("editType",sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", type.id);
                    cmd.Parameters.AddWithValue("name", type.name);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();
                }
            }
        }

        public async Task DeleteType(Models.Type type)
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