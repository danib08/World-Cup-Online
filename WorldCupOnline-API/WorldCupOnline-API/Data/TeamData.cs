
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class TeamData
    {
        DbConection con = new DbConection();
        public async Task <List<Team>> GetTeams()
        {
            var list = new List<Team>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("get_teams", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using(var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var team = new Team();
                            team.id = (string)item["id"];
                            team.name = (string)item["name"];
                            team.confederation = (string)item["confederation"];
                            team.typeid = (int)item["typeid"];
                            list.Add(team);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Team>> GetOneTeam(Team data)
        {
            var list = new List<Team>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOneTeam", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var team = new Team();
                            team.id = (string)item["id"];
                            team.name = (string)item["name"];
                            team.confederation = (string)item["confederation"];
                            team.typeid = (int)item["typeid"];
                            list.Add(team);

                        }
                    }
                }

                return list;
            }
        }

        public async Task<List<TeamTypeBody>> GetType(Team data)
        {
                var list = new List<TeamTypeBody>();
                using (var sql = new SqlConnection(con.SQLCon()))
                {
                    using (var cmd = new SqlCommand("getTypeTeam", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@typeid", data.typeid);

                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                var team = new TeamTypeBody();
                                team.id = (string)item["id"];
                                team.label = (string)item["label"];
                                list.Add(team);

                            }
                        }
                    }

                    return list;
                }
            }


        public async Task PostTeams(Team team)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("insertTeam", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", team.id);
                    cmd.Parameters.AddWithValue("@name", team.name);
                    cmd.Parameters.AddWithValue("@confederation", team.confederation);
                    cmd.Parameters.AddWithValue("@typeid", team.typeid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task PutTeam(Team team)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editTeam", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", team.id);
                    cmd.Parameters.AddWithValue("@name", team.name);
                    cmd.Parameters.AddWithValue("@confederation", team.confederation);
                    cmd.Parameters.AddWithValue("@typeid", team.typeid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

  

        public async Task DeleteTeam(Team team)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_team", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", team.id);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

    }
}
