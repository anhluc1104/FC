using FootBallWeb.Models;
using FootBallWeb.Services;
using FootBallWeb.Services.ServicesImpl;
using Microsoft.EntityFrameworkCore;

namespace FootBallWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ??ng ký AppDbContext và k?t n?i t?i SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<PlayerService, PlayerServiceImpl>();
            builder.Services.AddScoped<TeamService, TeamServiceImpl>();
            builder.Services.AddScoped<PlayerHistoryService, PlayerHistoryServiceImpl>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
