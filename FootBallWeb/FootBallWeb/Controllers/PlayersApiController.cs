using FootBallWeb.Models.DTO;
using FootBallWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace FootBallWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Sẽ là: /api/playersapi
    public class PlayersApiController : Controller
    {
        private readonly PlayerService _playerService;

        public PlayersApiController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET: api/playersapi
        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            try
            {
                var players = await _playerService.GetAllPlayersIsDeleteFalse();
                var result = players.Select(p => new PlayerDTO
                {
                    Id = p.PlayerId,
                    Name = p.Name,
                    Nationality = p.Nationality,
                    Position = p.Position
                });// Định dạng ngày tháng
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi server", detail = ex.Message });
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
