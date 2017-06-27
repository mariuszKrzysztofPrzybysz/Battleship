using System.Threading.Tasks;
using System.Web.Mvc;
using BattleShip.Repository.Interfaces;

namespace BattleShip.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IPlayerRepository _playerRepository;

        public ChatController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        // GET: Chat
        public async Task<ActionResult> Index()
        {
            var accountName = User.Identity.Name;

            var viewModel = await _playerRepository.GetAllOnlinePlayersExceptAsync(accountName);

            return View(viewModel);
        }
    }
}