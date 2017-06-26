using System.Threading.Tasks;
using System.Web.Mvc;
using BattleShip.Repository.Interfaces;

namespace BattleShip.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public ChatController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: Chat
        public async Task<ActionResult> Index()
        {
            var accountName = User.Identity.Name;

            var viewModel = await _accountRepository.GetAllOnlinePlayersExceptAsync(accountName);

            return View(viewModel);
        }
    }
}