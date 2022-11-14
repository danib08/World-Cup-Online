using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Team_In_TournamentData
    {

        DbConection con = new DbConection();
        public async Task<List<Team_In_Tournament>> GetTeam_In_Tournament()
        {
            var list = new List<Team_In_Tournament>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("get_TIT", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var team_In_Tournament = new Team_In_Tournament();
                            team_In_Tournament.teamid = (string)item["teamid"];
                            team_In_Tournament.tournamentid = (int)item["tournamentid"];
                            list.Add(team_In_Tournament);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Team_In_Tournament>> GetOneTeam_In_Tournament(Team_In_Tournament data)
        {
            var list = new List<Team_In_Tournament>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOneTIT", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", data.teamid);
                    cmd.Parameters.AddWithValue("@tournamentid", data.tournamentid);
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var team_In_Tournament = new Team_In_Tournament();
                            team_In_Tournament.teamid = (string)item["teamid"];
                            team_In_Tournament.tournamentid = (int)item["tournamentid"];
                            list.Add(team_In_Tournament);

                        }
                    }
                }

                return list;
            }
        }

        public async Task PostTeam_In_Tournament(Team_In_Tournament team_In_Tournament)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("insertTIT", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", team_In_Tournament.teamid);
                    cmd.Parameters.AddWithValue("@tournamentid", team_In_Tournament.tournamentid);

                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task DeleteTeam_In_Tournament(Team_In_Tournament team_In_Tournament)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_TIT", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@teamid", team_In_Tournament.teamid);
                    cmd.Parameters.AddWithValue("@tournamentid", team_In_Tournament.tournamentid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }
    }
}
