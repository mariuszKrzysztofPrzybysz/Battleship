using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.ViewModels;
using BattleShip.Web.Helpers;

namespace BattleShip.Web.Controllers
{
    public class BattleController : Controller
    {
        private readonly IBattleRepository _battleRepository;

        public BattleController(IBattleRepository battleRepository)
        {
            _battleRepository = battleRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(string playerName)
        {
            var result = await _battleRepository.CreateAsync(new CreateBattleViewModel
            {
                PlayerName = playerName,
                OpponentName = User.Identity.Name,
                StartUtcDateTime = DateTime.UtcNow
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ActionName("Play")]
        public async Task<ActionResult> PlayAsync(long battleId)
        {
            var playerName = User.Identity.Name;

            var access = await _battleRepository.CheckAccessAsync(battleId, playerName);

            if (!access.IsSuccess)
                return RedirectToAction("Index", "Player");

            return View(access.Data as PlayBattleViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> UploadBoardAsync(long battleId, string board)
        {
            var playerName = User.Identity.Name;

            var result = await _battleRepository.UploadBoardAsync(battleId, playerName, board);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AttackAsync(long battleId, string cell)
        {
            var playerName = User.Identity.Name;

            var result = await _battleRepository.AttackAsync(battleId, playerName, cell);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GiveInAsync(long battleId)
        {
            var playerName = User.Identity.Name;

            var result = await _battleRepository.GiveInAsync(battleId, playerName);

            if (result.IsSuccess)
                result.Data = UrlBuilder.GetUrl("Chat", "Index");

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}