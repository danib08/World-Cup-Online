using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Data
{
    public class TournamentData
    {
        private readonly DbConection _con = new();

        public async Task<List<GetTournamentBody>> GetTournament()
        {
            var list = new List<GetTournamentBody>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTournaments", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    var tournament = new GetTournamentBody
                    {
                        id = (int)item["id"],
                        name = (string)item["name"],
                        startdate = (DateTime)item["startdate"],
                        enddate = (DateTime)item["enddate"],
                        description = (string)item["description"],
                        type = (string)item["type"]
                    };
                    list.Add(tournament);
                }
            }
            return list;
        }

        public async Task<GetTournamentBody> GetOneTournament(int id)
        {
            var tournament = new GetTournamentBody();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneTournament", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    tournament = new GetTournamentBody
                    {
                        id = (int)item["id"],
                        name = (string)item["name"],
                        startdate = (DateTime)item["startdate"],
                        enddate = (DateTime)item["enddate"],
                        description = (string)item["description"],
                        type = (string)item["type"]
                    };
                }
            }
            return tournament;
        }

        public async Task<List<MatchTournamentBody>> GetMatchesByTournament(int id)
        {
            var list = new List<MatchTournamentBody>();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getMatchesByTournament", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var item = await cmd.ExecuteReaderAsync();
                while (await item.ReadAsync())
                {
                    var tournamentMatches = new MatchTournamentBody
                    {
                        id = (int)item["id"],
                        name = (string)item["name"],
                        startdate = (DateTime)item["startdate"],
                        starttime = (TimeSpan)item["starttime"],
                        location = (string)item["location"],
                        state = (string)item["state"],
                        score = (string)item["score"]
                    };
                    list.Add(tournamentMatches);
                }
            }
            return list;
        }

        public async Task<List<ValueIntBody>> GetPhasesByTournament(int id)
        {
            var list = new List<ValueIntBody>();
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getPhasesByTournament", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var item = new ValueIntBody
                    {
                        value = (int)reader["value"],
                        label = (string)reader["label"]
                    };
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task<List<TeamTournamentBody>> GetTeamsByTournament(int id)
        {
            var list = new List<TeamTournamentBody>();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getTeamsByTournament", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var team = new TeamTournamentBody
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        confederation = (string)reader["confederation"]
                    };
                    list.Add(team);
                }
            }
            return list;
        }

        public async Task CreateTournament(TournamentCreator tournament)
        {
            int newTournamentId = 0;
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertTournament", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", tournament.name);
            cmd.Parameters.AddWithValue("@startdate", tournament.startdate);
            cmd.Parameters.AddWithValue("@enddate", tournament.enddate);
            cmd.Parameters.AddWithValue("@description", tournament.description);
            cmd.Parameters.AddWithValue("@typeid", tournament.typeid);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader["id"];
                newTournamentId = Convert.ToInt32(id);
            }

            await reader.CloseAsync();
            await sql.CloseAsync();

            var teams = new Team_In_TournamentData();
            foreach (string teamid in tournament.teamsIds)
            {
                var TIM = new Team_In_Tournament
                {
                    teamid = teamid,
                    tournamentid = newTournamentId
                };

                await teams.CreateTeam_In_Tournament(TIM);
            }

            var phases = new PhaseData();
            foreach (string item in tournament.phases)
            {
                var phase = new Phase
                {
                    name = item,
                    tournamentID = newTournamentId
                };
                await phases.CreatePhase(phase);
            }
        }

        public async Task EditTournament(int id, Tournament tournament)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editTournament", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", tournament.name);
            cmd.Parameters.AddWithValue("@startdate", tournament.startdate);
            cmd.Parameters.AddWithValue("@enddate", tournament.enddate);
            cmd.Parameters.AddWithValue("@description", tournament.description);
            cmd.Parameters.AddWithValue("@typeid", tournament.typeid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task DeleteTournament(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteTournament", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
