using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Interfaces;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Data
{
    public class LeagueData : ILeagueData
    {
        ///Create new connenction
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all Leagues
        /// </summary>
        /// <returns>List of League objects</returns>
        public async Task <List<League>> GetLeagues()
        {
            var list = new List<League>();///Creates a List of League objects
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getLeague", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    var league = new League
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        accesscode = (string)reader["accesscode"],
                        tournamentid = (string)reader["tournamentid"],
                        userid = (string)reader["userid"]
                    };
                    list.Add(league);
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return list; ///return list
        }

        /// <summary>
        /// Method to obtain only one League
        /// </summary>
        /// <param name="id"></param>
        /// <returns>League object</returns>
        public async Task<League> GetOneLeague(string id)
        {
            var league = new League();///Creates object League
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneLeague", sql))///Calls stored procedure via sql connection
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add parameters with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from Database
                    league = new League
                    {
                        id = (string)reader["id"],
                        name = (string)reader["name"],
                        accesscode = (string)reader["accesscode"],
                        tournamentid = (string)reader["tournamentid"],
                        userid = (string)reader["userid"]
                    };
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return league; ///Return object
        }

        /// <summary>
        /// Method to obtain all Tournaments
        /// </summary>
        /// <returns>List of ValueIntBody object</returns>
        public async Task<List<ValueStringBody>> GetTournaments()
        {
            var list = new List<ValueStringBody>();///Create list of ValueIntBody object

            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getTournLeagues", sql);///Calls stored procedure via sql connection
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ///Read from database
                    var type = new ValueStringBody
                    {
                        value = (string)reader["value"],
                        label = (string)reader["label"]
                    };
                    list.Add(type); ///Add to list
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to create League
        /// </summary>
        /// <param name="league"></param>
        /// <returns></returns>
        public async Task<string> CreateLeague(LeagueCreator league)
        {
            string isInLeague = "";
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmdExists = new SqlCommand("isInTournamentLeague", sql);

            cmdExists.CommandType = CommandType.StoredProcedure;
            cmdExists.Parameters.AddWithValue("@userid", league.userid);
            cmdExists.Parameters.AddWithValue("@tournamentid", league.tournamentid);

            await sql.OpenAsync();
            using var readerExists = await cmdExists.ExecuteReaderAsync();

            while (await readerExists.ReadAsync())
            {
                isInLeague = (string)readerExists["isInLeague"];      
            }

            await readerExists.CloseAsync();

            if (isInLeague == "FALSE")
            {
                Random random = new();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string newLeagueId = new(Enumerable.Repeat(chars, 6)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());

                string accessCode = league.tournamentid + newLeagueId;

                using var cmd = new SqlCommand("insertLeague", sql);///Calls stored procedure via sql connection

                cmd.CommandType = CommandType.StoredProcedure;///Indicates that command is a stored procedure
                                                              ///Add parameters with value
                cmd.Parameters.AddWithValue("@id", newLeagueId);
                cmd.Parameters.AddWithValue("@name", league.name);
                cmd.Parameters.AddWithValue("@accesscode", accessCode);
                cmd.Parameters.AddWithValue("@tournamentid", league.tournamentid);
                cmd.Parameters.AddWithValue("@userid", league.userid);

                using var reader = await cmd.ExecuteReaderAsync();

                await reader.CloseAsync();
                await sql.CloseAsync();

                return accessCode;
            }
            else
            {
                await sql.CloseAsync();
                var message = "FAIL";
                return message;
            }
        }
    }
}
