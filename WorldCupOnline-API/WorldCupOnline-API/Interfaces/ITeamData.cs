using WorldCupOnline_API.Models;
using WorldCupOnline_API.Bodies;

namespace WorldCupOnline_API.Interfaces
{
    public interface ITeamData
    {
        Task<List<IdStringBody>> GetTeams();
        Task<Team> GetOneTeam(string id);
        Task<List<IdStringBody>> GetTeamsByType(int type);
        Task<List<IdStringBody>> GetPlayersByTeam(string id);
    }
}
