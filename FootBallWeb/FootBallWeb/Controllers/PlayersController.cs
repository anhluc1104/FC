using FootBallWeb.Models;
using FootBallWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FootBallWeb.Controllers
{
    public class PlayersController : Controller
    {
        private readonly PlayerService _playerService;
        private readonly TeamService _teamService;
        private readonly PlayerHistoryService _playerHistoryService;
        private readonly FootballApiService _apiService = new FootballApiService(); // Khởi tạo dịch vụ API

        public PlayersController(PlayerService playerService, TeamService teamService, PlayerHistoryService playerHistoryService)
        {
            _playerService = playerService;
            _teamService = teamService;
            _playerHistoryService = playerHistoryService;
        }

        public async Task<IActionResult> Index()
        {
            var players = await _playerService.GetAllPlayersIsDeleteFalse();

            return View(players); // ← Truyền dữ liệu sang View
        }

        public async Task<IActionResult> Create()
        {

            ViewBag.DuplicateMessage = ""; // Khởi tạo thông báo trống
            if (TempData["DuplicateMessage"] != null)
            {
                ViewBag.DuplicateMessage = TempData["DuplicateMessage"].ToString();
            }
            ViewBag.Teams = new SelectList(await _teamService.GetAllTeamsAsyncAndIsDeleteFalse(), "TeamId", "Name");
            return View();
        }

        // POST: Players/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            ViewBag.DuplicateMessage = ""; // Khởi tạo thông báo trống
            if (ModelState.IsValid && player != null)
            {
                if (await PlayerExists(player.Name, player.Age, player.Nationality))
                {
                    ViewBag.Teams = new SelectList(await _teamService.GetAllTeamsAsyncAndIsDeleteFalse(), player.TeamId);
                    ViewBag.DuplicateMessage = "Cầu thủ đã tồn tại. Không thể thêm mới.";
                    return View(player);
                }
                await _playerService.Add(player);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"❌ {state.Key} => {error.ErrorMessage}");
                    }
                }
            }
            ViewBag.Teams = new SelectList(await _teamService.GetAllTeamsAsyncAndIsDeleteFalse(), "TeamId", "Name"); // Nếu có lỗi form, vẫn truyền lại
            return View(player);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var player = await _playerService.GetPlayerByIdAsyncAndIsDeleteFalse(id);
            if (player == null) return NotFound();

            ViewBag.Teams = new SelectList(await _teamService.GetAllTeamsAsyncAndIsDeleteFalse(), "TeamId", "Name", player.TeamId);
            return View(player);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.PlayerId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPlayer = await _playerService.GetPlayerByIdAsyncAndIsDeleteFalse(id);
                    if (existingPlayer == null)
                        return NotFound();

                    // Cập nhật các thuộc tính (trừ CreatedAt)
                    existingPlayer.Name = player.Name;
                    existingPlayer.Position = player.Position;
                    existingPlayer.Age = player.Age;
                    existingPlayer.Nationality = player.Nationality;
                    existingPlayer.PhotoUrl = player.PhotoUrl;
                    existingPlayer.TeamId = player.TeamId;
                    existingPlayer.UpdatedAt = DateTime.Now; // ← chỉ cập nhật thời gian sửa

                    await _playerService.Update(existingPlayer);
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingPlayer = await _playerService.GetPlayerByIdAsyncAndIsDeleteFalse(id);
                    if (existingPlayer == null)
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewBag.Teams = new SelectList(await _teamService.GetAllTeamsAsyncAndIsDeleteFalse(), player.TeamId);
            return View(player);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var player = await _playerService.GetPlayerByIdAsyncAndIsDeleteFalse(id);
            if (player == null) return NotFound();

            return View(player);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerService.GetPlayerByIdAsyncAndIsDeleteFalse(id);
            if (player != null)
            {
                await _playerService.Delete(player);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _playerService.GetPlayerByIdAsyncAndIsDeleteFalse(id);

            if (player == null)
                return NotFound();

            var histories = await _playerHistoryService.GetAllPlayerHistoriesAsyncAndIsDeleteFalse();

            // Truyền vào View qua ViewBag hoặc ViewModel (nếu cần)
            ViewBag.PlayerTeamHistory = histories;
            ViewBag.Teams = new SelectList(await _teamService.GetAllTeamsAsyncAndIsDeleteFalse(), "TeamId", "Name");
            return View(player); // truyền player sang View
        }
        public async Task<IActionResult> CallAPI(string name)
        {
            ViewBag.DuplicateMessage = ""; // Khởi tạo thông báo trống
            bool isHaveOne = false;
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Vui lòng nhập tên cầu thủ";
                return RedirectToAction("Create"); // hoặc View(...)
            }
            var players = await _apiService.GetPlayerByNameAsync(name);
            foreach (var item in players)
            {

                Player player = new Player
                {
                    Name = item.Name,
                    Position = item.Position,
                    Age = DateTime.Now.Year - DateTime.Parse(item.BirthDate).Year,
                    Nationality = item.Nationality,
                    PhotoUrl = item.Photo,
                    TeamId = 1,
                    CreatedAt = DateTime.Now,
                    isDeleted = false// Giả sử bạn đã có ID đội bóng, hoặc có thể lấy từ danh sách đội bóng
                };
                if (await PlayerExists(player.Name, player.Age, player.Nationality))
                {
                    continue;
                }
                await _playerService.Add(player);
                isHaveOne = true; // Nếu có ít nhất một cầu thủ được thêm vào
                                  // Thêm vào cơ sở dữ liệu
            }
            if (!isHaveOne)
            {
                //ViewBag.DuplicateMessage = "Cầu thủ đã tồn tại. Không thể thêm mới.";
                //ViewBag.Teams = new SelectList(await _teamService.GetAllTeamsAsyncAndIsDeleteFalse(), "TeamId", "Name");
                //return View(); // Trả về trang Create nếu không tìm thấy cầu thủ
                TempData["DuplicateMessage"] = "Cầu thủ đã tồn tại. Không thể thêm mới.";
                return RedirectToAction("Create"); // Trả về trang Create nếu không tìm thấy cầu thủ
            }
            return RedirectToAction("Index");// Trả về View Create với dữ liệu player đã lấy từ API
        }
        private async Task<bool> PlayerExists(string name, int age, string nationality)
        {
            return await _playerService.IsPlayerExists(name, age, nationality);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Nếu không có từ khóa tìm kiếm, trả về danh sách tất cả cầu thủ
                var players = await _playerService.GetAllPlayersIsDeleteFalse();
                return View("Index", players); // Trả về view Index với danh sách tất cả cầu thủ
            }
            else
            {
                // Nếu có từ khóa tìm kiếm, gọi dịch vụ để tìm cầu thủ theo quốc tịch
                keyword = keyword.Trim(); // Loại bỏ khoảng trắng đầu và cuối
                var players = await _playerService.GetPlayersByNationlityAsync(keyword);
                return View("Index", players); // Trả về view Index với kết quả
            }
        }
    }
}
