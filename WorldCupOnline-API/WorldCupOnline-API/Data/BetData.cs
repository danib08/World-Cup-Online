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
        private readonly DbConnection _con = new();

        public async Task<List<Bet>> GetBets()
        {
            var list = new List<Bet>();
            using (var sql = new SqlConnection(_con.SQLCon()))
            {
                using var cmd = new SqlCommand("getBets", sql);
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var bet = new Bet
                    {
                        id = (int)reader["id"],
                        goalsteam1 = (int)reader["goalsteam1"],
                        goalsteam2 = (int)reader["goalsteam2"],
                        score = (int)reader["score"],
                        mvp = (string)reader["mvp"],
                        userid = (string)reader["userid"],
                        matchid = (int)reader["matchid"],
                    };
                    list.Add(bet);
                }
            }
            return list;
        }

        public async Task<Bet> GetOneBet(int id)
        {
            var bet = new Bet();
            using var sql = new SqlConnection(_con.SQLCon());
            using (var cmd = new SqlCommand("getOneBet", sql))
            {
                await sql.OpenAsync();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    bet = new Bet
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
            return bet;
        }

        public async Task CreateBet(string userId, int matchId, BetCreator bet)
        {
            int newBetId = 0;
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("insertBet", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@goalsteam1", bet.team1goals);
            cmd.Parameters.AddWithValue("@goalsteam2", bet.team2goals);
            cmd.Parameters.AddWithValue("@mvp", bet.mvpid);
            cmd.Parameters.AddWithValue("@userid", userId);
            cmd.Parameters.AddWithValue("@matchid", matchId);

            await sql.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader["id"];
                newBetId = Convert.ToInt32(id);
            }

            await reader.CloseAsync();
            await sql.CloseAsync();
            var scorers = new Scorer_In_BetData();
            var assists = new Assist_In_BetData();

            foreach (string scorerId in bet.team1scorers)
            {
                var SIB = new Scorer_In_Bet
                {
                    betid = newBetId,
                    playerid = scorerId
                };
                await scorers.CreateScorer_In_Bet(SIB);
            }

            foreach (string scorerId in bet.team2scorers)
            {
                var SIB = new Scorer_In_Bet
                {
                    betid = newBetId,
                    playerid = scorerId
                };
                await scorers.CreateScorer_In_Bet(SIB);
            }

            foreach (string assistId in bet.team1assists)
            {
                var AIB = new Assist_In_Bet
                {
                    betid = newBetId,
                    playerid = assistId
                };
                await assists.CreateAssist_In_Bet(AIB);
            }

            foreach (string assistId in bet.team2assists)
            {
                var AIB = new Assist_In_Bet
                {
                    betid = newBetId,
                    playerid = assistId
                };
                await assists.CreateAssist_In_Bet(AIB);
            }
        }

        public async Task EditBet(int id, Bet bet)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("editBet", sql);

            cmd.CommandType = CommandType.StoredProcedure;
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

        public async Task DeleteBet(int id)
        {
            using var sql = new SqlConnection(_con.SQLCon());
            using var cmd = new SqlCommand("deleteBet", sql);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await sql.OpenAsync();
            await cmd.ExecuteReaderAsync();
        }
    }
}
