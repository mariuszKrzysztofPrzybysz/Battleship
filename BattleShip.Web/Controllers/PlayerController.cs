﻿using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;
using BattleShip.Web.Profiles;
using BattleShip.Web.ViewModels;

namespace BattleShip.Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPlayerRepository _playerRepository;

        static PlayerController()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new PlayerProfile()));
        }

        public PlayerController(IAccountRepository accountRepository, IPlayerRepository playerRepository)
        {
            _accountRepository = accountRepository;
            _playerRepository = playerRepository;
        }

        // GET: Player
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync()
        {
            var login = User.Identity.Name;

            var account = await _accountRepository.GetAccountAsync(login);

            if (account == null)
                return RedirectToAction("Index");

            var model = Mapper.Map<Account, ExtendedEditAccountViewModel>(account);

            return View(model);
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ExtendedEditAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.AccountPhoto != null)
            {
                if (model.AccountPhoto.ContentLength > 1 * 1024 * 1024)
                {
                    ModelState.AddModelError("AccountPhoto", "Image can not be lager than 1MB.");
                    return View(model);
                }
                if (!(model.AccountPhoto.ContentType == "image/jpeg"
                      || model.AccountPhoto.ContentType == "image/png"))
                {
                    ModelState.AddModelError("AccountPhoto", "Image must be in jpeg or png format.");
                    return View(model);
                }

                var data = new byte[model.AccountPhoto.ContentLength];
                await model.AccountPhoto.InputStream.ReadAsync(data, 0, model.AccountPhoto.ContentLength);
                model.Photo = data;
            }

            var result = await _accountRepository.UpdateAccountAsync(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            return RedirectToAction("Index", "Player");
        }

        public ActionResult Battles()
        {
            return View();
        }
    }
}