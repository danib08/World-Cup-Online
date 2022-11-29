using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Interfaces
{
    public interface IMatchData
    {
        Task<List<Match>> GetMatches();
        Task<Match> GetOneMatch(int id);
        Task CreateMatch(MatchCreator match);
        Task EditMatch(int id, BetCreator match);
    }
}
