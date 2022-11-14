using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class TeamData
    {
        private readonly DbConection _con = new();

        public async Task <List<IdStringBody>> GetTeams()
        {
            var list = new List<IdStringBody>();

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTeams", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var team = new IdStringBody
                    {
                        id = (string)reader["id"],
                        label = (string)reader["label"]
                    };
                    list.Add(team);
                }
            }
            return list;
        }

        public async Task<Team> GetOneTeam(string id)
        {
            var team = new Team();
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOneTeam", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    team = new Team
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        confederation = (string)reader["confederation"],
                        typeid = (int)reader["typeid"]
                    };
                }
            }
            return team;
        }

        public async Task<List<IdStringBody>> GetTeamsByType(int type)
        {
            var list = new List<IdStringBody>();

            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getTeamsByType", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@typeid", type);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var item = new IdStringBody
                    {
                        id = (string)reader["id"],
                        label = (string)reader["label"]
                    };
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task CreateTeam(Team team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTeam", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", team.id);
            cmd.Parameters.AddWithValue("@name", team.name);
            cmd.Parameters.AddWithValue("@confederation", team.confederation);
            cmd.Parameters.AddWithValue("@typeid", team.typeid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task EditTeam(string id, Team team)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editTeam", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", team.name);
            cmd.Parameters.AddWithValue("@confederation", team.confederation);
            cmd.Parameters.AddWithValue("@typeid", team.typeid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteTeam(string id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteTeam", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
