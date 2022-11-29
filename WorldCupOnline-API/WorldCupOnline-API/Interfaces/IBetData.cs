using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Interfaces
{
    public interface IBetData
    {
        Task<List<Bet>> GetBets();
        Task<Bet> GetOneBet(int id);
        Task CreateBet(string userId, int matchId, BetCreator bet);
    }
}
