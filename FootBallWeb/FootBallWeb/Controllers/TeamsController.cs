using FootBallWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FootBallWeb.Controllers
{
    public class TeamsController : Controller
    {
        private readonly AppDbContext _context;
        public TeamsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams.ToListAsync();
            return View(teams); // ← Truyền dữ liệu sang View
        }
        // GET: Upsert
        [HttpGet]
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.Teams = new SelectList(await _context.Teams.ToListAsync(), "TeamId", "Name");

            if (id == null)
            {
                return View(new Team()); // ← Tạo mới
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            return View(team); // ← Cập nhật
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Teams = new SelectList(await _context.Teams.ToListAsync(), "TeamId", "Name", team.TeamId);
                return View(team);
            }

            if (team.TeamId == 0)
            {
                team.CreatedAt = DateTime.Now; // Thêm thời gian tạo
                team.UpdatedAt = DateTime.Now; // Thêm thời gian cập nhật
                _context.Teams.Add(team); // Tạo mới
            }
            else
            {
                var existingTeam = await _context.Teams.FindAsync(team.TeamId);
                if (existingTeam == null)
                {
                    return NotFound();
                }

                // Cập nhật từng trường bạn cho phép sửa
                existingTeam.Name = team.Name;
                existingTeam.Description = team.Description;
                existingTeam.Country = team.Country;
                existingTeam.city = team.city;
                existingTeam.stadium = team.stadium;
                existingTeam.LogoUrl = team.LogoUrl;

                // Cập nhật thời gian
                existingTeam.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
