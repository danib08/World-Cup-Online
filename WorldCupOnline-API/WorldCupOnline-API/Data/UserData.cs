using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class UserData
    {
        DbConection con = new DbConection();
        public async Task<List<Users>> GetUsers()
        {
            var list = new List<Users>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("get_users", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var user = new Users();
                            user.username = (string)item["username"];
                            user.name = (string)item["name"];
                            user.lastname = (string)item["lastname"];
                            user.email = (string)item["email"];
                            user.countryid = (string)item["countryid"];
                            user.birthdate = (DateTime)item["birthdate"];
                            user.password = (string)item["password"];
                            list.Add(user);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Users>> GetOneUser(Users data)
        {
            var list = new List<Users>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOneUser", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", data.username);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var user = new Users();
                            user.username = (string)item["username"];
                            user.name = (string)item["name"];
                            user.lastname = (string)item["lastname"];
                            user.email = (string)item["email"];
                            user.countryid = (string)item["countryid"];
                            user.birthdate = (DateTime)item["birthdate"];
                            user.password = (string)item["password"];
                            list.Add(user);

                        }
                    }
                }

                return list;
            }
        }

        public async Task PostUsers(Users user)
            {
                using (var sql = new SqlConnection(con.SQLCon()))
                {
                    using (var cmd = new SqlCommand("insertUser", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", user.username);
                        cmd.Parameters.AddWithValue("@name", user.name);
                        cmd.Parameters.AddWithValue("@lastname", user.lastname);
                        cmd.Parameters.AddWithValue("@email", user.email);
                        cmd.Parameters.AddWithValue("@countryid", user.countryid);
                        cmd.Parameters.AddWithValue("@birthdate", user.birthdate);
                        cmd.Parameters.AddWithValue("@password", user.password);
                        await sql.OpenAsync();
                        await cmd.ExecuteReaderAsync();

                    }
                }
            }

        public async Task PutUser(Users user)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editUser", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@name", user.name);
                    cmd.Parameters.AddWithValue("@lastname", user.lastname);
                    cmd.Parameters.AddWithValue("@email", user.email);
                    cmd.Parameters.AddWithValue("@countryid", user.countryid);
                    cmd.Parameters.AddWithValue("@birthdate", user.birthdate);
                    cmd.Parameters.AddWithValue("@password", user.password);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task DeleteUser(Users user)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_user", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", user.username);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

    }
}
