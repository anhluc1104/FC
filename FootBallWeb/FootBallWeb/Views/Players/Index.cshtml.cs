using FootBallWeb.Models;
using FootBallWeb.Services;
using FootBallWeb.Services.ServicesImpl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootBallWeb.Views.Players
{
    public class IndexModel : PageModel
    {
        private readonly PlayerServiceImpl _playerService;

        public IndexModel(PlayerServiceImpl playerService)
        {
            _playerService = playerService;
        }
        [BindProperty]
        public string SearchKeyword { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public async Task OnGetAsync()
        {
            // Load mặc định tất cả
            Players = await _playerService.GetAllPlayersAsync();
        }
        //public async Task OnPostAsync()
        //{
        //    // Nếu người dùng nhập từ khóa thì tìm kiếm, không thì load tất cả
        //    Players = string.IsNullOrWhiteSpace(SearchKeyword)
        //        ? await _playerService.GetPlayersByNationlityAsync("")
        //        : await _playerService.GetPlayersByNationlityAsync(SearchKeyword);
        //}
        public async Task OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                Players = await _playerService.GetPlayersByNationlityAsync(SearchKeyword);
            }
            else
            {
                Players = await _playerService.GetAllPlayersAsync();
            }
        }
    }
}
