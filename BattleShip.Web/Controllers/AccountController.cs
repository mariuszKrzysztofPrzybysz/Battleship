using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

            if (!result.IsSuccess)
            {
                //TODO: Poinformować o błędzie
                //throw new NotImplementedException();
                ModelState.AddModelError(result.ErrorMessage, new Exception());
                return View();
            }

            FormsAuthentication.SetAuthCookie(viewModel.Login.ToLower(), true);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            var viewModel=new SignInAccountViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var authenticatedAccount = _repository
                .AuthenticateAccount(viewModel.Login, viewModel.Password);

            if (!authenticatedAccount.IsSuccess)
            {
                ModelState.AddModelError(authenticatedAccount.ErrorMessage,new Exception());
                return View();
            }

            FormsAuthentication.SetAuthCookie(viewModel.Login.ToLower(), true);
            return RedirectToAction("Index", "Player");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}