using FootBallWeb.Models;

namespace FootBallWeb.Services
{
    public interface PlayerService
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<List<Player>> GetAllPlayersIsDeleteFalse();
        Task<Player?> GetPlayerByIdAsync(int id);
        Task<Player?> GetPlayerByIdAsyncAndIsDeleteFalse(int? id);
        Task Add(Player player);
        Task Update(Player player);
        Task Delete(int? id);
        Task SaveChangesAsync();
        Task Delete(Player player);
        Task<List<Player>> GetPlayersByNationlityAsync(string name);

        Task<bool> IsPlayerExists(string name, int age, string nationality);
    }
}
