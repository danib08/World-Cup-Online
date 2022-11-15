using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Models;
using System.Security.Cryptography;
using System.Text;
using WorldCupOnline_API.Connection;

namespace WorldCupOnline_API.Data
{
    public class UserData
    {
        private readonly DbConnection _con = new();
        public async Task<List<Users>> GetUsers()
        {
            var list = new List<Users>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getUsers", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var user = new Users
                    {
                        username = (string)reader["username"],
                        name = (string)reader["name"],
                        lastname = (string)reader["lastname"],
                        email = (string)reader["email"],
                        countryid = (string)reader["countryid"],
                        birthdate = (DateTime)reader["birthdate"],
                        password = (string)reader["password"]
                    };
                    list.Add(user);
                }
            }
            return list;
        }

        public async Task<Users> GetOneUser(string username)
        {
            var user = new Users();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneUser", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", username);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    user = new Users
                    {
                        username = (string)reader["username"],
                        name = (string)reader["name"],
                        lastname = (string)reader["lastname"],
                        email = (string)reader["email"],
                        countryid = (string)reader["countryid"],
                        birthdate = (DateTime)reader["birthdate"],
                        password = (string)reader["password"]
                    };
                }
            }
            return user;
        }

        public async Task CreateUsers(Users user)
        {
            byte[] bytesPassword = Encoding.ASCII.GetBytes(user.password);
            byte[] hash;

            using (SHA512 shaM = SHA512.Create())
            {
                hash = shaM.ComputeHash(bytesPassword);
            }

            string encrypted = Convert.ToBase64String(hash);

            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertUser", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", user.username);
            cmd.Parameters.AddWithValue("@name", user.name);
            cmd.Parameters.AddWithValue("@lastname", user.lastname);
            cmd.Parameters.AddWithValue("@email", user.email);
            cmd.Parameters.AddWithValue("@countryid", user.countryid);
            cmd.Parameters.AddWithValue("@birthdate", user.birthdate);
            cmd.Parameters.AddWithValue("@password", encrypted);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task<string> AuthUser(Auth auth)
        {
            string storedPassword = "";
            string storedUsername = "";
            string username = "";

            byte[] bytesPassword = Encoding.ASCII.GetBytes(auth.password);
            byte[] hash;

            using (SHA512 hasher = SHA512.Create())
            {
                hash = hasher.ComputeHash(bytesPassword);
            }

            string encrypted = Convert.ToBase64String(hash);

            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("authUser", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", auth.email);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                storedPassword = (string)reader["password"];
                storedUsername = (string)reader["username"];
            }

            if (storedPassword == encrypted)
            {
                username = storedUsername;
            }
            return username;
        }

        public async Task EditUser(string username, Users user)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editUser", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@name", user.name);
            cmd.Parameters.AddWithValue("@lastname", user.lastname);
            cmd.Parameters.AddWithValue("@email", user.email);
            cmd.Parameters.AddWithValue("@countryid", user.countryid);
            cmd.Parameters.AddWithValue("@birthdate", user.birthdate);
            cmd.Parameters.AddWithValue("@password", user.password);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteUser(string username)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteUser", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", username);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
