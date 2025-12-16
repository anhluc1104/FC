using FootBallWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FootBallWeb.Services.ServicesImpl
{
    public class TeamServiceImpl : TeamService
    {
        private readonly AppDbContext _context;
        public TeamServiceImpl(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }
        public async Task<List<Team>> GetAllTeamsAsyncAndIsDeleteFalse()
        {
            return await _context.Teams
                .Where(t => t.isDeleted == false)
                .ToListAsync();
        }
        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams.FindAsync(id);
        }
        public async Task<Team?> GetTeamByIdAsyncAndIsDeleteFalse(int? id)
        {
            return await _context.Teams
                .Where(t => t.TeamId == id && t.isDeleted == false)
                .FirstOrDefaultAsync();
        }
        public async Task AddTeamAsync(Team team)
        {
            team.CreatedAt = DateTime.Now;
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateTeamAsync(Team team)
        {
            team.UpdatedAt = DateTime.Now;
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTeamAsync(int id)
        {
            var team = await GetTeamByIdAsync(id);
            if (team != null)
            {
                team.isDeleted = true;
                team.UpdatedAt = DateTime.Now;
                _context.Teams.Update(team);
                await _context.SaveChangesAsync();
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
