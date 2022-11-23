using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WorldCupOnline_API.Connection;
using WorldCupOnline_API.Models;


namespace WorldCupOnline_API.Data
{
    public class BetData
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
            await sql.CloseAsync();

            ///Call methods from other Data files
            var scorers = new Scorer_In_BetData();
            var assists = new Assist_In_BetData();

            ///Create Scorer in bet for team1 for each scorer
            foreach (string scorerId in bet.team1scorers)
            {
                var SIB = new Scorer_In_Bet
                {
                    betid = newBetId,
                    playerid = scorerId
                };
                await scorers.CreateScorer_In_Bet(SIB); ///Create Scorer in Bet
            }

            ///Create Scorer in bet for team2 for each scorer
            foreach (string scorerId in bet.team2scorers)
            {
                var SIB = new Scorer_In_Bet
                {
                    betid = newBetId,
                    playerid = scorerId
                };
                await scorers.CreateScorer_In_Bet(SIB); ///Create Scorer in Bet
            }

            ///Create Assist in bet for team1 for each assist
            foreach (string assistId in bet.team1assists)
            {
                var AIB = new Assist_In_Bet
                {
                    betid = newBetId,
                    playerid = assistId
                };
                await assists.CreateAssist_In_Bet(AIB);///Create Assist in Bet
            }

            ///Create Assist in bet for team2 for each assist
            foreach (string assistId in bet.team2assists)
            {
                var AIB = new Assist_In_Bet
                {
                    betid = newBetId,
                    playerid = assistId
                };
                await assists.CreateAssist_In_Bet(AIB);///Create Assist in Bet
            }
        }

        /// <summary>
        /// Method to edit a bet
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bet"></param>
        /// <returns></returns>
        public async Task EditBet(int id, Bet bet)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editBet", sql); ///Calls the stored procedure

            cmd.CommandType = CommandType.StoredProcedure; //Indicates that command is a stored procedure
            ///Add parameters with value for bet
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@goalsteam1", bet.goalsteam1);
            cmd.Parameters.AddWithValue("@goalsteam2", bet.goalsteam2);
            cmd.Parameters.AddWithValue("@score", bet.score);
            cmd.Parameters.AddWithValue("@mvp", bet.mvp);
            cmd.Parameters.AddWithValue("@userid", bet.userid);
            cmd.Parameters.AddWithValue("@matchid", bet.matchid);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }

        /// <summary>
        /// Method to delete a bet by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteBet(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteBet", sql); ///Calls the stored procedure

            cmd.CommandType = CommandType.StoredProcedure; //Indicates that command is a stored procedure
            cmd.Parameters.AddWithValue("@id", id); ///Add parameter with value

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
