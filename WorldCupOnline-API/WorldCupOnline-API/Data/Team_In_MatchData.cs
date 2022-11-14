using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Team_In_MatchData
    {
        private readonly DbConection _con = new();

        public async Task<List<Team_In_Match>> GetTeam_In_Match()
        {
            var list = new List<Team_In_Match>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTIM", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var team_In_Match = new Team_In_Match
                    {
                        teamid = (string)reader["teamid"],
                        matchid = (int)reader["matchid"]
                    };
                    list.Add(team_In_Match);
                }
            }
            return list;
        }

        public async Task<List<Team_In_Match>> GetOneTeam_In_Match(string teamid, int matchid)
        {
            var list = new List<Team_In_Match>();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneTIM", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@teamid", teamid);
                cmd.Parameters.AddWithValue("@matchid", matchid);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var team_In_Match = new Team_In_Match
                    {
                        teamid = (string)reader["teamid"],
                        matchid = (int)reader["matchid"]
                    };
                    list.Add(team_In_Match);
                }
            }
            return list;
        }

        public async Task CreateTeam_In_Match(Team_In_Match team_In_Match)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTIM", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamid", team_In_Match.teamid);
            cmd.Parameters.AddWithValue("@matchid", team_In_Match.matchid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteTeam_In_Match(string teamid, int matchid)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("delete_TIM", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamid", teamid);
            cmd.Parameters.AddWithValue("@matchid", matchid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
