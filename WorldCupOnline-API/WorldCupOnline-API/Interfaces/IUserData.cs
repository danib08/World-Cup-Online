using WorldCupOnline_API.Bodies;
using WorldCupOnline_API.Models;

namespace WorldCupOnline_API.Interfaces
{
    public interface IUserData
    {
        Task<List<Users>> GetUsers();
        Task<Users> GetOneUser(string username);
        Task CreateUsers(Users user);
        Task<string> AuthUser(Auth auth);
        Task<List<ValueStringBody>> GetCountries();
    }
}
