using System.Data.SqlClient;
using System.Data;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class PhaseData
    {
        ///Create connection
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to get all phases
        /// </summary>
        /// <returns>List of all Phases</returns>
        public async Task <List<Phase>> GetPhases()
        {
            var list = new List<Phase>(); ///Create list of Phase object

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getPhase", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    var phase = new Phase///Create Phase object
                    {
                        id = (int)reader["id"],
                        name = (string)reader["name"],
                        tournamentID = (int)reader["tournamentid"]
                    };
                    list.Add(phase);/// Add object to list
                }
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain a phase
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Phase object</returns>
        public async Task<Phase> GetOnePhase(int id)
        {
            var phase = new Phase();
            using var sql = new SqlConnection(_con.SQLCon());

            using (var cmd = new SqlCommand("getOnePhase", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    phase = new Phase();
                    phase.id = (int)reader["id"];
                    phase.name = (string)reader["name"];
                    phase.tournamentID = (int)reader["tournamentid"];
                }
            }
            return phase;
        }

        /// <summary>
        /// Method to create a phase
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        public async Task CreatePhase(Phase phase)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertPhase", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@name", phase.name);
            cmd.Parameters.AddWithValue("@tournamentid", phase.tournamentID);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to edit a phase
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phase"></param>
        /// <returns></returns>
        public async Task EditPhase(int id, Phase phase)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editPhase", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            ///Add parameters with value
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", phase.name);
            cmd.Parameters.AddWithValue("@typeid", phase.tournamentID);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
      
        /// <summary>
        /// Method to delete phase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePhase(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deletePhase", sql);///Calls stored procedure via sql connection

            cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedre
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
