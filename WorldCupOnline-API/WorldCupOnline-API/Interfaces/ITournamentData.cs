using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Interfaces
{
    public interface ITournamentData
    {
        Task<List<GetTournamentBody>> GetTournament();
        Task<GetTournamentBody> GetOneTournament(string id);
        Task<List<MatchTournamentBody>> GetMatchesByTournament(string id);
        Task<List<ValueIntBody>> GetPhasesByTournament(string id);
        Task<List<TeamTournamentBody>> GetTeamsByTournament(string id);
        Task CreateTournament(TournamentCreator tournament);
        Task CreateTeam_In_Tournament(Team_In_Tournament team_In_Tournament);
        Task CreatePhase(Phase phase);
    }
}
