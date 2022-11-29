using System.Collections.Generic;
using System.Data.SqlClient;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class PhaseData
    {
        DbConection con = new DbConection();
        public async Task <List<Phase>> GetPhases()
        {
            var list = new List<Phase>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("get_phase", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using(var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var phase = new Phase();
                            phase.id = (int)item["id"];
                            phase.name = (string)item["name"];
                            phase.tournamentID = (int)item["tournamentid"];
                            list.Add(phase);
                        }
                    }
                }
            }
            return list;
        }

        public async Task<List<Phase>> GetOnePhase(Phase data)
        {
            var list = new List<Phase>();
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("getOnePhase", sql))
                {
                    await sql.OpenAsync();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", data.id);

                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while(await item.ReadAsync())
                        {
                            var phase = new Phase();
                            phase.id = (int)item["id"];
                            phase.name = (string)item["name"];
                            phase.tournamentID = (int)item["tournamentid"];
                            list.Add(phase);

                        }
                    }
                }

                return list;
            }
        }

        public async Task PostPhase(Phase phase)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using(var cmd = new SqlCommand("insertPhase", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", phase.id);
                    cmd.Parameters.AddWithValue("@name", phase.name);
                    cmd.Parameters.AddWithValue("@tournamentid", phase.tournamentID);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

        public async Task PutPhase(Phase phase)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("editPhase", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", phase.id);
                    cmd.Parameters.AddWithValue("@name", phase.name);
                    cmd.Parameters.AddWithValue("@typeid", phase.tournamentID);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

       

        public async Task DeletePhase(Phase phase)
        {
            using (var sql = new SqlConnection(con.SQLCon()))
            {
                using (var cmd = new SqlCommand("delete_phase", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", phase.id);
                    await sql.OpenAsync();
                    await cmd.ExecuteReaderAsync();

                }
            }
        }

    }
}
