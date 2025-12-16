using FootBallWeb.Models;
namespace FootBallWeb.Services
{
    public interface TeamService
    {
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<Team>> GetAllTeamsAsyncAndIsDeleteFalse();
        Task<Team?> GetTeamByIdAsync(int id);
        Task<Team?> GetTeamByIdAsyncAndIsDeleteFalse(int? id);
        Task AddTeamAsync(Team team);
        Task UpdateTeamAsync(Team team);
        Task DeleteTeamAsync(int id);
        Task SaveChangesAsync();
    }
}
