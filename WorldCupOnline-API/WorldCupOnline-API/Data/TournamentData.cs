using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Data
{
    public class TournamentData
    {
        DbConection con = new DbConection();
        public async Task<List<Tournament>> GetTournament()
        {
            var list = new List<Tournament>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("get_tournaments", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var tournament = new Tournament();
                            tournament.id = (int)item["id"];
                            tournament.name = (string)item["name"];
                            tournament.startdate = (DateTime)item["startdate"];
                            tournament.enddate = (DateTime)item["enddate"];
                            tournament.description = (string)item["description"];
                            tournament.typeid = (int)item["typeid"];
                            list.Add(tournament);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Tournament>> GetOneTournament(Tournament data)
        {
            var list = new List<Tournament>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOneTournament", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var tournament = new Tournament();
                            tournament.id = (int)item["id"];
                            tournament.name = (string)item["name"];
                            tournament.startdate = (DateTime)item["startdate"];
                            tournament.enddate = (DateTime)item["enddate"];
                            tournament.description = (string)item["description"];
                            tournament.typeid = (int)item["typeid"];
                            list.Add(tournament);

                        }
                    }
                }

                return list;
            }
        }

        public async Task<List<MatchTournamentBody>> GetMatchesTournament(Tournament data)
        {
            var list = new List<MatchTournamentBody>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getMatchesTournament", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var tournamentMatches = new MatchTournamentBody();
                            tournamentMatches.id = (int)item["id"];
                            tournamentMatches.name = (string)item["name"];
                            tournamentMatches.startdate = (DateTime)item["startdate"];
                            tournamentMatches.starttime = (TimeSpan)item["starttime"];
                            tournamentMatches.location = (string)item["location"];
                            tournamentMatches.state = (string)item["state"];
                            tournamentMatches.score = (string)item["score"];
                            list.Add(tournamentMatches);

                        }
                    }
                }

                return list;
            }
        }

        public async Task<List<PhasesBody>> GetPhasesTournament(Tournament data)
        {
            var list = new List<PhasesBody>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getPhasesTournament", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var tournament = new PhasesBody();
                            tournament.value = (int)item["value"];
                            tournament.label = (string)item["label"];
                            list.Add(tournament);

                        }
                    }
                }

                return list;
            }
        }

        public async Task<List<TeamTournamentBody>> GetTeamsTournament(Tournament data)
        {
            var list = new List<TeamTournamentBody>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getTeamsTournament", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var tournament = new TeamTournamentBody();
                            tournament.id = (string)item["id"];
                            tournament.name = (string)item["name"];
                            tournament.confederation = (string)item["confederation"];
                            list.Add(tournament);

                        }
                    }
                }
                return list;
            }
        }

        public async Task PostTournament(TournamentCreator tournament)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("insertTournament", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", tournament.name);
                    cmd.Parameters.AddWithValue("@startdate", tournament.startdate);
                    cmd.Parameters.AddWithValue("@enddate", tournament.enddate);
                    cmd.Parameters.AddWithValue("@description", tournament.description);
                    cmd.Parameters.AddWithValue("@typeid", tournament.typeid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task PutTournament(Tournament tournament)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editTournament", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", tournament.id);
                    cmd.Parameters.AddWithValue("@name", tournament.name);
                    cmd.Parameters.AddWithValue("@startdate", tournament.startdate);
                    cmd.Parameters.AddWithValue("@enddate", tournament.enddate);
                    cmd.Parameters.AddWithValue("@description", tournament.description);
                    cmd.Parameters.AddWithValue("@typeid", tournament.typeid);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task DeleteTournament(Tournament tournament)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_tournament", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", tournament.id);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }
    }
}
