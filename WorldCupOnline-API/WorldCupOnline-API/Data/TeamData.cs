using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class TeamData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all teams
        /// </summary>
        /// <returns>List of IdStringBody objects</returns>
        public async Task <List<IdStringBody>> GetTeams()
        {
            var list = new List<IdStringBody>(); ///Create list of IdStringBody object

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTeams", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var team = new IdStringBody
                    {
                        id = (string)reader["id"],
                        label = (string)reader["label"]
                    };
                    list.Add(team); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain one team
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Team object</returns>
        public async Task<Team> GetOneTeam(string id)
        {
            var team = new Team(); ///Create Team object
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOneTeam", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);///Add value with parameters

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    team = new Team
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        confederation = (string)reader["confederation"],
                        typeid = (int)reader["typeid"]
                    };
                }
            }
            return team; ///Return object
        }

        /// <summary>
        /// Method to obtain all teams by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns>List of IdStringBody objects</returns>
        public async Task<List<IdStringBody>> GetTeamsByType(int type)
        {
            var list = new List<IdStringBody>(); ///Create IdStringBody list

            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getTeamsByType", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@typeid", type);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var item = new IdStringBody
                    {
                        id = (string)reader["id"],
                        label = (string)reader["label"]
                    };
                    list.Add(item); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain all teams by type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of IdStringBody objects</returns>
        public async Task<List<IdStringBody>> GetPlayersByTeam(string id)
        {
            var list = new List<IdStringBody>(); ///Create IdStringBody list

            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getPlayersByTeam", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var item = new IdStringBody
                    {
                        id = (string)reader["id"],
                        label = (string)reader["label"]
                    };
                    list.Add(item); ///Add to list
                }
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to create Team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task CreateTeam(Team team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTeam", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", team.id);
            cmd.Parameters.AddWithValue("@name", team.name);
            cmd.Parameters.AddWithValue("@confederation", team.confederation);
            cmd.Parameters.AddWithValue("@typeid", team.typeid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit a team
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public async Task EditTeam(string id, Team team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editTeam", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", team.name);
            cmd.Parameters.AddWithValue("@confederation", team.confederation);
            cmd.Parameters.AddWithValue("@typeid", team.typeid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete Teams
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteTeam(string id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteTeam", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id);///Ad parameter with value

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
