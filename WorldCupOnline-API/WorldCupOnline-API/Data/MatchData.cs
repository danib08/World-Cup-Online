using System.Collections.Generic;
using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class MatchData
    {
        DbConection con = new DbConection();
        public async Task <List<Match>> GetMatches()
        {
            var list = new List<Match>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("get_matches", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using(var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var match = new Match();
                            match.id = (string)item["id"];
                            match.startdate = (DateTime)item["startdate"];
                            match.starttime = (DateTime)item["starttime"];
                            match.score = (string)item["score"];
                            match.location = (string)item["location"];
                            match.stateid = (int)item["stateid"];
                            match.tournamentid = (int)item["tournamentid"];
                            match.phaseid = (int)item["phaseid"];
                            list.Add(match);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Match>> GetOneMatch(Match data)
        {
            var list = new List<Match>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOneMatch", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var match = new Match();
                            match.id = (string)item["id"];
                            match.startdate = (DateTime)item["startdate"];
                            match.starttime = (DateTime)item["starttime"];
                            match.score = (string)item["score"];
                            match.location = (string)item["location"];
                            match.stateid = (int)item["stateid"];
                            match.tournamentid = (int)item["tournamentid"];
                            match.phaseid = (int)item["phaseid"];
                            list.Add(match);

                        }
                    }
                }

                return list;
            }
        }

        public async Task PostMatch(Match match)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("insertMatch", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", match.id);
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
            }
        }

        public async Task PutMatch(Match match)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editMatch", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", match.id);
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
            }
        }

        public async Task DeleteMatch(Match match)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_match", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", match.id);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

    }
}
