using FootBallWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FootBallWeb.Services.ServicesImpl
{
    public class PlayerServiceImpl : PlayerService
    {
        private readonly AppDbContext _context;

        public PlayerServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }
        public async Task<List<Player>> GetAllPlayersAsyncAndIsDeleteFalse()
        {
            return await _context.Players
                .Include(p => p.Team)
                .Where(p => p.isDeleted == false)
                .ToListAsync();
        }
        public async Task<Player?> GetPlayerByIdAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }
        public async Task<Player?> GetPlayerByIdAsyncAndIsDeleteFalse(int? id)
        {
            return await _context.Players
                .Where(p => p.PlayerId == id && p.isDeleted == false)
                .FirstOrDefaultAsync();
        }
        public async Task Add(Player player)
        {
            player.CreatedAt = DateTime.Now;
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Player player)
        {
            player.UpdatedAt = DateTime.Now;
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var player = await GetPlayerByIdAsync(id);
            if (player != null)
            {
                player.isDeleted = true;
                player.UpdatedAt = DateTime.Now;
                _context.Players.Update(player);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(Player player)
        {
            if (player != null)
            {
                player.isDeleted = true;
                player.UpdatedAt = DateTime.Now;
                _context.Players.Update(player);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Player>> GetAllPlayersIsDeleteFalse()
        {
            return await _context.Players
                .Include(p => p.Team)
                .Where(p => p.isDeleted == false)
                .ToListAsync();
        }

        public Task Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsPlayerExists(string name, int age, string nationality)
        {
            return await _context.Players
                .AnyAsync(p => EF.Functions.Like(p.Name, $"%{name}%") && p.Age == age && p.Nationality == nationality);

        }
        public async Task<List<Player>> GetPlayersByNationlityAsync(string name)
        {
            return await _context.Players
                .FromSqlRaw("EXEC GetPlayersByCountry @p0", name)
                .ToListAsync();
        }
    }
}
