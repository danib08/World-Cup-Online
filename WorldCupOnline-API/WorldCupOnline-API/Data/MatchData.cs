using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class MatchData
    {
        private readonly DbConnection _con = new();

        public async Task <List<Match>> GetMatches()
        {
            var list = new List<Match>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getMatches", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var match = new Match
                    {
                        id = (int)reader["id"],
                        startdate = (DateTime)reader["startdate"],
                        starttime = (TimeSpan)reader["starttime"],
                        score = (string)reader["score"],
                        location = (string)reader["location"],
                        stateid = (int)reader["stateid"],
                        tournamentid = (int)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"]
                    };
                    list.Add(match);
                }
            }
            return list;
        }

        public async Task<Match> GetOneMatch(int id)
        {
            var match = new Match();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneMatch", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    match = new Match
                    {
                        id = (int)reader["id"],
                        startdate = (DateTime)reader["startdate"],
                        starttime = (TimeSpan)reader["starttime"],
                        score = (string)reader["score"],
                        location = (string)reader["location"],
                        stateid = (int)reader["stateid"],
                        tournamentid = (int)reader["tournamentid"],
                        phaseid = (int)reader["phaseid"]
                    };
                }
            }
            return match;
        }

        public async Task CreateMatch(MatchCreator match)
        {
            int newMatchId = 0;
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertMatch", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@startdate", match.startdate);
            cmd.Parameters.AddWithValue("@starttime", match.starttime);
            cmd.Parameters.AddWithValue("@score", "0-0");
            cmd.Parameters.AddWithValue("@location", match.location);
            cmd.Parameters.AddWithValue("@stateid", 1);
            cmd.Parameters.AddWithValue("@tournamentid", match.tournamentid);
            cmd.Parameters.AddWithValue("@phaseid", match.phaseid);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader["id"];
                newMatchId = Convert.ToInt32(id);
            }

            await reader.CloseAsync();
            await sql.CloseAsync();
            var teams = new Team_In_MatchData();

            var team1 = new Team_In_Match
            {
                teamid = match.team1,
                matchid = newMatchId
            };

            var team2 = new Team_In_Match
            {
                teamid = match.team2,
                matchid = newMatchId
            };

            await teams.CreateTeam_In_Match(team1);
            await teams.CreateTeam_In_Match(team2);
        }

        public async Task EditMatch(int id, Match match)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editMatch", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@startdate", match.startdate);
            cmd.Parameters.AddWithValue("@starttime", match.starttime);
            cmd.Parameters.AddWithValue("@score", match.score);
            cmd.Parameters.AddWithValue("@location", match.location);
            cmd.Parameters.AddWithValue("@stateid", match.stateid);
            cmd.Parameters.AddWithValue("@tournamentid", match.tournamentid);
            cmd.Parameters.AddWithValue("@phaseid", match.phaseid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteMatch(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("delete_match", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

    }
}
