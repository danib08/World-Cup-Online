using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Team_In_MatchData
    {

        DbConection con = new DbConection();
        public async Task<List<Team_In_Match>> GetTeam_In_Match()
        {
            var list = new List<Team_In_Match>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("get_TIM", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var team_In_Match = new Team_In_Match();
                            team_In_Match.teamid = (string)item["teamid"];
                            team_In_Match.matchid = (int)item["matchid"];
                            list.Add(team_In_Match);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Team_In_Match>> GetOneTeam_In_Match(Team_In_Match data)
        {
            var list = new List<Team_In_Match>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOneTIM", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", data.teamid);
                    cmd.Parameters.AddWithValue("@matchid", data.matchid);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var team_In_Match = new Team_In_Match();
                            team_In_Match.teamid = (string)item["teamid"];
                            team_In_Match.matchid = (int)item["matchid"];
                            list.Add(team_In_Match);

                        }
                    }
                }

                return list;
            }
        }

       

        public async Task PostTeam_In_Match(Team_In_Match team_In_Match)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("insertTIM", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", team_In_Match.teamid);
                    cmd.Parameters.AddWithValue("@matchid", team_In_Match.matchid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task PutTeam_In_Match(Team_In_Match team_In_Match)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editTim", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", team_In_Match.teamid);
                    cmd.Parameters.AddWithValue("@matchid", team_In_Match.matchid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }



        public async Task DeleteTeam_In_Match(Team_In_Match team_In_Match)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_TIM", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", team_In_Match.teamid);
                    cmd.Parameters.AddWithValue("@matchid", team_In_Match.matchid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }
    }
}
