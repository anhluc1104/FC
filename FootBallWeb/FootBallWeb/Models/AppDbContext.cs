using Microsoft.EntityFrameworkCore;

namespace FootBallWeb.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerTeamHistory> PlayerTeamHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Player → Team (Mỗi cầu thủ hiện tại thuộc 1 đội)
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade

            // PlayerTeamHistory → Player (Một lịch sử thuộc về một cầu thủ)
            modelBuilder.Entity<PlayerTeamHistory>()
                .HasOne(h => h.Player)
                .WithMany(p => p.PlayerTeamHistories)
                .HasForeignKey(h => h.PlayerId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh cascade

            // PlayerTeamHistory → Team (Một lịch sử gắn với một đội)
            modelBuilder.Entity<PlayerTeamHistory>()
                .HasOne(h => h.Team)
                .WithMany(t => t.PlayerTeamHistories)
                .HasForeignKey(h => h.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Match → HomeTeam
            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict); // Tắt cascade

            // Match → AwayTeam
            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict); // Tắt cascade
        
        }
    }
}
