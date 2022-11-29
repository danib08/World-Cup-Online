using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Interfaces
{
    public interface ITournamentData
    {
        Task<List<GetTournamentBody>> GetTournament();
        Task<GetTournamentBody> GetOneTournament(string id);
        Task<List<ValueIntBody>> GetTypes();
        Task<List<MatchTournamentBody>> GetMatchesByTournament(string id);
        Task<List<ValueIntBody>> GetPhasesByTournament(string id);
        Task<List<IdStringBody>> GetTeamsByTournament(string id);
        Task CreateTournament(TournamentCreator tournament);
    }
}
