using System.Web.Mvc;
using BattleShip.Repository.Interfaces;

namespace BattleShip.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IAccountRepository _repository;

        public ChatController(IAccountRepository repository)
        {
            _repository = repository;
        }

        // GET: Chat
        public ActionResult Index()
        {
            var accountName = User.Identity.Name;

            var viewModel = _repository.GetOnlinePlayersExcept(accountName);

            return View(viewModel);
        }
    }
}