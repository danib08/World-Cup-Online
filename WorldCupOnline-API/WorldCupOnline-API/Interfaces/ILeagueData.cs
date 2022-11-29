using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Interfaces
{
    public interface ILeagueData
    {
        Task<List<League>> GetLeagues();
        Task<League> GetOneLeague(string id);
        Task<List<ValueStringBody>> GetTournaments();
        Task<string> CreateLeague(LeagueCreator league);
    }
}
