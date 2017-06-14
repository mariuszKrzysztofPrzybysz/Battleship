using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using BattleShip.Repository.Interfaces;
using BattleShip.Web.ViewModels;

namespace BattleShip.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            var viewModel = new SignUpAccountViewModel();

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUpAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Password = string.Empty;
                viewModel.ConfirmPassword = string.Empty;

                return View(viewModel);
            }

            var result = _repository.Add(viewModel);

            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TODO: Poinformować o błędzie
                //throw new NotImplementedException();
                throw new NotImplementedException();
            }
        }
    }
}