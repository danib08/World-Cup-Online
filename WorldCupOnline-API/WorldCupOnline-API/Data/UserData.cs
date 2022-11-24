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
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all users
        /// </summary>
        /// <returns>Lsit of users</returns>
        public async Task<List<Users>> GetUsers()
        {
            var list = new List<Users>(); ///Create list of users
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getUsers", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var user = new Users
                    {
                        username = (string)reader["username"],
                        name = (string)reader["name"],
                        lastname = (string)reader["lastname"],
                        email = (string)reader["email"],
                        countryid = (string)reader["countryid"],
                        birthdate = (DateTime)reader["birthdate"],
                        isadmin = (int)reader["isadmin"],
                        password = (string)reader["password"]
                    };
                    list.Add(user); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain one user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>User object</returns>
        public async Task<Users> GetOneUser(string username)
        {
            var user = new Users(); ///bject creation
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneUser", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@username", username); ///Add parameter with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    user = new Users
                    {
                        username = (string)reader["username"],
                        name = (string)reader["name"],
                        lastname = (string)reader["lastname"],
                        email = (string)reader["email"],
                        countryid = (string)reader["countryid"],
                        birthdate = (DateTime)reader["birthdate"],
                        isadmin = (int)reader["isadmin"],
                        password = (string)reader["password"]
                    };
                }
            }
            return user;///Return object
        }

        /// <summary>
        /// Method to create users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateUsers(Users user)
        {
            ///Arrays creation for password encryption
            byte[] bytesPassword = Encoding.ASCII.GetBytes(user.password);
            byte[] hash;

            using (SHA512 shaM = SHA512.Create())
            {
                hash = shaM.ComputeHash(bytesPassword); ///Encrypting password with Sha512
            }

            ///Convertion to base64
            string encrypted = Convert.ToBase64String(hash);

            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertUser", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@username", user.username);
            cmd.Parameters.AddWithValue("@name", user.name);
            cmd.Parameters.AddWithValue("@lastname", user.lastname);
            cmd.Parameters.AddWithValue("@email", user.email);
            cmd.Parameters.AddWithValue("@countryid", user.countryid);
            cmd.Parameters.AddWithValue("@birthdate", user.birthdate);
            cmd.Parameters.AddWithValue("@isadmin", user.isAdmin);
            cmd.Parameters.AddWithValue("@password", encrypted);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to authenticate users
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>string username</returns>
        public async Task<string> AuthUser(Auth auth)
        {
            ///Stored data initialization
            string storedPassword = "";
            string storedUsername = "";
            string username = "";

            ///Arrays for password encryption
            byte[] bytesPassword = Encoding.ASCII.GetBytes(auth.password);
            byte[] hash;

            using (SHA512 hasher = SHA512.Create())
            {
                hash = hasher.ComputeHash(bytesPassword); ///Encrypting password
            }

            string encrypted = Convert.ToBase64String(hash);

            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("authUser", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@email", auth.email);/// Add param with value

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                storedPassword = (string)reader["password"];
                storedUsername = (string)reader["username"];
            }

            ///Validation of existance
            if (storedPassword == encrypted)
            {
                username = storedUsername;
            }
            return username; ///Returns username
        }

        /// <summary>
        /// Method to edit users
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task EditUser(string username, Users user)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editUser", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add params with values
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@name", user.name);
            cmd.Parameters.AddWithValue("@lastname", user.lastname);
            cmd.Parameters.AddWithValue("@email", user.email);
            cmd.Parameters.AddWithValue("@countryid", user.countryid);
            cmd.Parameters.AddWithValue("@birthdate", user.birthdate);
            cmd.Parameters.AddWithValue("@isadmin", user.isAdmin);
            cmd.Parameters.AddWithValue("@password", user.password);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete users
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task DeleteUser(string username)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteUser", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@username", username);///Add param with value

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
