using System.Data;
using System.Data.SqlClient;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;
using WorldCupOnline_API.Interfaces;

namespace WorldCupOnline_API.Data
{
    public class BetData : IBetData
    {
        ///Create new connenction
        private readonly DbConnection _con = new();

        /// <summary>
        /// Method to obtain all Bets
        /// </summary>
        /// <returns>A list of objects Bet</returns>
        public async Task<List<Bet>> GetBets()
        {
            var list = new List<Bet>(); ///Creates a List of Bet objects
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getBets", sql); ///Calls the stored procedure
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure; //Indicates that command is a stored procedure

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var bet = new Bet
                    ///Read data form database
                    {
                        id = (int)reader["id"],
                        goalsteam1 = (int)reader["goalsteam1"],
                        goalsteam2 = (int)reader["goalsteam2"],
                        score = (int)reader["score"],
                        mvp = (string)reader["mvp"],
                        userid = (string)reader["userid"],
                        matchid = (int)reader["matchid"],
                    };
                    list.Add(bet); ///Add object to list
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return list; ///Return list
        }

        /// <summary>
        /// Method to obtain one specific bet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bet object</returns>
        public async Task<Bet> GetOneBet(int id)
        {
            var bet = new Bet(); ///Creates object Bet
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneBet", sql)) ///Calls the stored procedure
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure; //Indicates that command is a stored procedure
                cmd.Parameters.AddWithValue("@id", id); ///Add aprameter with value

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    bet = new Bet
                    ///Read data form database
                    {
                        id = (int)reader["id"],
                        goalsteam1 = (int)reader["goalsteam1"],
                        goalsteam2 = (int)reader["goalsteam2"],
                        score = (int)reader["score"],
                        mvp = (string)reader["mvp"],
                        userid = (string)reader["userid"],
                        matchid = (int)reader["matchid"],
                    };
                }
                await reader.CloseAsync();
                await sql.CloseAsync();
            }
            return bet; ///Return object
        }

        /// <summary>
        /// Method to create a Bet
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="matchId"></param>
        /// <param name="bet"></param>
        /// <returns></returns>
        public async Task CreateBet(string userId, int matchId, BetCreator bet)
        {
            int newBetId = 0;
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertBet", sql); ///Calls the stored procedure

            cmd.CommandType = CommandType.StoredProcedure; //Indicates that command is a stored procedure
            ///Add parameters with value of Bet
            cmd.Parameters.AddWithValue("@goalsteam1", bet.team1goals);
            cmd.Parameters.AddWithValue("@goalsteam2", bet.team2goals);
            cmd.Parameters.AddWithValue("@mvp", bet.mvpid);
            cmd.Parameters.AddWithValue("@userid", userId);
            cmd.Parameters.AddWithValue("@matchid", matchId);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            ///Read from Database
            {
                var id = reader["id"];
                newBetId = Convert.ToInt32(id);
            }

            await reader.CloseAsync();

            //Create Scorer_In_Bet for Team 1
            foreach (string scorerId in bet.team1scorers)
            {
                using var cmdScorer1 = new SqlCommand("insertSIB", sql);
                cmdScorer1.CommandType = CommandType.StoredProcedure;
                cmdScorer1.Parameters.AddWithValue("@betid", newBetId);
                cmdScorer1.Parameters.AddWithValue("@playerid", scorerId);

                using var readerScorer1 = await cmdScorer1.ExecuteReaderAsync();
                await readerScorer1.CloseAsync();
            }

            //Create Scorer_In_Bet for Team 2
            foreach (string scorerId in bet.team2scorers)
            {
                using var cmdScorer2 = new SqlCommand("insertSIB", sql);
                cmdScorer2.CommandType = CommandType.StoredProcedure;
                cmdScorer2.Parameters.AddWithValue("@betid", newBetId);
                cmdScorer2.Parameters.AddWithValue("@playerid", scorerId);

                using var readerScorer2 = await cmdScorer2.ExecuteReaderAsync();
                await readerScorer2.CloseAsync();
            }

            //Create Assist_In_Bet for Team 1
            foreach (string assistId in bet.team1assists)
            {
                using var cmdAssist1 = new SqlCommand("insertAIB", sql);
                cmdAssist1.CommandType = CommandType.StoredProcedure;
                cmdAssist1.Parameters.AddWithValue("@betid", newBetId);
                cmdAssist1.Parameters.AddWithValue("@playerid", assistId);

                using var readerAssist1 = await cmdAssist1.ExecuteReaderAsync();
                await readerAssist1.CloseAsync();
            }

            //Create Assist_In_Bet for Team 2
            foreach (string assistId in bet.team2assists)
            {
                using var cmdAssist2 = new SqlCommand("insertAIB", sql);
                cmdAssist2.CommandType = CommandType.StoredProcedure;
                cmdAssist2.Parameters.AddWithValue("@betid", newBetId);
                cmdAssist2.Parameters.AddWithValue("@playerid", assistId);

                using var readerAssist2 = await cmdAssist2.ExecuteReaderAsync();
                await readerAssist2.CloseAsync();
            }
            await sql.CloseAsync();
        }
    }
}
