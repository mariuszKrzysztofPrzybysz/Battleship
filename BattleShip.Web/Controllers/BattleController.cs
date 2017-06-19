using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Web.Controllers
{
    public class BattleController : Controller
    {
        private readonly IBattleRepository _repository;

        public BattleController(IBattleRepository repository)
        {
            _repository = repository;
        }

        //[HttpPost]
        public async Task<ActionResult> Create(string opponentName)
        {
            var result = await _repository.CreateAsync(new CreateBattleViewModel
            {
                PlayerName = User.Identity.Name,
                OpponentName = opponentName,
                StartUtcDateTime = DateTime.UtcNow
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UploadBoard(long battleId, string board)
        {
            var userName = User.Identity.Name;

            var result = await _repository.UploadBoardAsync(battleId, userName, board);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Attack(long battleId, string cell)
        {
            var attackerName = User.Identity.Name;

            var result = await _repository.AttackAsync(battleId, attackerName, cell);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}