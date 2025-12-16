using FootBallWeb.Models;
using Microsoft.EntityFrameworkCore;
namespace FootBallWeb.Services.ServicesImpl
{
    public class PlayerHistoryServiceImpl : PlayerHistoryService
    {
        private readonly AppDbContext _context;
        public PlayerHistoryServiceImpl(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<PlayerTeamHistory>> GetAllPlayerHistoriesAsync()
        {
            return await _context.PlayerTeamHistories.ToListAsync();
        }
        public async Task<List<PlayerTeamHistory>> GetAllPlayerHistoriesAsyncAndIsDeleteFalse()
        {
            return await _context.PlayerTeamHistories
                .Where(ph => ph.isDeleted == false)
                .ToListAsync();
        }
        public async Task<PlayerTeamHistory?> GetPlayerHistoryByIdAsync(int id)
        {
            return await _context.PlayerTeamHistories.FindAsync(id);
        }
        public async Task<PlayerTeamHistory?> GetPlayerHistoryByIdAsyncAndIsDeleteFalse(int? id)
        {
            return await _context.PlayerTeamHistories
                .Where(ph => ph.Id == id && ph.isDeleted == false)
                .FirstOrDefaultAsync();
        }
        public async Task AddPlayerHistoryAsync(PlayerTeamHistory playerHistory)
        {
            playerHistory.CreatedAt = DateTime.Now;
            _context.PlayerTeamHistories.Add(playerHistory);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePlayerHistoryAsync(PlayerTeamHistory playerHistory)
        {
            playerHistory.UpdatedAt = DateTime.Now;
            _context.PlayerTeamHistories.Update(playerHistory);
            await _context.SaveChangesAsync();
        }
        public async Task DeletePlayerHistoryAsync(int id)
        {
            var playerHistory = await GetPlayerHistoryByIdAsync(id);
            if (playerHistory != null)
            {
                playerHistory.isDeleted = true;
                playerHistory.UpdatedAt = DateTime.Now;
                _context.PlayerTeamHistories.Update(playerHistory);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<PlayerTeamHistory>> GetPlayerHistoriesByPlayerIdAsync(int playerId)
        {
            return await _context.PlayerTeamHistories
                .Where(ph => ph.PlayerId == playerId && ph.isDeleted == false)
                .Include(h => h.Team)
                .OrderByDescending(h => h.StartDate)
                .ToListAsync();
        }
    }
}
