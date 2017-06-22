using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.ViewModels;
using BattleShip.Web.Helpers;
using BattleShip.Web.ViewModels;

namespace BattleShip.Web.Controllers
{
    public class BattleController : Controller
    {
        private readonly IBattleRepository _repository;

        public BattleController(IBattleRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Play(long id)
        {
            var access = await _repository.CheckAccessAsync(id, User.Identity.Name);

            if (!access.IsSuccess)
                return RedirectToAction("Index", "Error");

            return View(access.Data as PlayBattleViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string playerName)
        {
            var result = await _repository.CreateAsync(new CreateBattleViewModel
            {
                PlayerName = playerName,
                OpponentName = User.Identity.Name,
                StartUtcDateTime = DateTime.UtcNow
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadBoard(long battleId, string board)
        {
            var userName = User.Identity.Name;

            var result = await _repository.UploadBoardAsync(battleId, userName, board);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Attack(long battleId, string cell)
        {
            var attackerName = User.Identity.Name;

            var result = await _repository.AttackAsync(battleId, attackerName, cell);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> GiveInAsync(long battleId)
        {
            var player = User.Identity.Name;

            var result = await _repository.GiveInAsync(battleId, player);

            if (result.IsSuccess)
                result.Data = UrlBuilder.GetUrl("Chat", "Index");
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}