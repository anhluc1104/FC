using FootBallWeb.Models;

namespace FootBallWeb.Services
{
    public interface PlayerHistoryService
    {
        Task<List<PlayerTeamHistory>> GetAllPlayerHistoriesAsync();
        Task<List<PlayerTeamHistory>> GetAllPlayerHistoriesAsyncAndIsDeleteFalse();
        Task<PlayerTeamHistory?> GetPlayerHistoryByIdAsync(int id);
        Task<PlayerTeamHistory?> GetPlayerHistoryByIdAsyncAndIsDeleteFalse(int? id);
        Task AddPlayerHistoryAsync(PlayerTeamHistory playerHistory);
        Task UpdatePlayerHistoryAsync(PlayerTeamHistory playerHistory);
        Task DeletePlayerHistoryAsync(int id);
        Task SaveChangesAsync();
        Task<List<PlayerTeamHistory>> GetPlayerHistoriesByPlayerIdAsync(int playerId);
    }
}
