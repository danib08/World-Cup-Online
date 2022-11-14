using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class Team_In_TournamentData
    {
        private readonly DbConection _con = new();

        public async Task<List<Team_In_Tournament>> GetTeam_In_Tournament()
        {
            var list = new List<Team_In_Tournament>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTIT", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var team_In_Tournament = new Team_In_Tournament
                    {
                        teamid = (string)reader["teamid"],
                        tournamentid = (int)reader["tournamentid"]
                    };
                    list.Add(team_In_Tournament);
                }
            }
            return list;
        }

        public async Task<Team_In_Tournament> GetOneTeam_In_Tournament(string teamid, int tournamentid)
        {
            var tit = new Team_In_Tournament();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneTIT", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@teamid", teamid);
                cmd.Parameters.AddWithValue("@tournamentid", tournamentid);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tit = new Team_In_Tournament
                    {
                        teamid = (string)reader["teamid"],
                        tournamentid = (int)reader["tournamentid"]
                    };
                }
            }
            return tit;
        }

        public async Task CreateTeam_In_Tournament(Team_In_Tournament team_In_Tournament)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTIT", sql);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamid", team_In_Tournament.teamid);
            cmd.Parameters.AddWithValue("@tournamentid", team_In_Tournament.tournamentid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteTeam_In_Tournament(string teamid, int tournamentid)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteTIT", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@teamid", teamid);
            cmd.Parameters.AddWithValue("@tournamentid", tournamentid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
