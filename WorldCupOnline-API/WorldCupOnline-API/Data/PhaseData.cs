using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Conection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class PhaseData
    {
        private readonly DbConection _con = new();

        public async Task <List<Phase>> GetPhases()
        {
            var list = new List<Phase>();

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getPhase", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var phase = new Phase
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        tournamentID = (int)reader["tournamentid"]
                    };
                    list.Add(phase);
                }
            }
            return list;
        }

        public async Task<Phase> GetOnePhase(int id)
        {
            var phase = new Phase();
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOnePhase", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    phase = new Phase();
                    phase.id = (int)reader["id"];
                    phase.name = (string)reader["name"];
                    phase.tournamentID = (int)reader["tournamentid"];
                }
            }
            return phase;
        }

        public async Task CreatePhase(Phase phase)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertPhase", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", phase.name);
            cmd.Parameters.AddWithValue("@tournamentid", phase.tournamentID);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        public async Task EditPhase(int id, Phase phase)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editPhase", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", phase.name);
            cmd.Parameters.AddWithValue("@typeid", phase.tournamentID);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
      
        public async Task DeletePhase(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deletePhase", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
