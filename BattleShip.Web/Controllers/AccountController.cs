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
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _repository;
        private readonly IAccountRoleRepository _accountRoleRepository;

        public AccountController(IAccountRepository repository, IAccountRoleRepository accountRoleRepository)
        {
            _repository = repository;
            _accountRoleRepository = accountRoleRepository;
        }

        // GET: Account
        public ActionResult SignUp()
        {
            var viewModel = new SignUpAccountViewModel();

            return View(viewModel);
        }

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

            SetAuthCookieAndAddSession(viewModel.Login.ToLower(), true);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignIn()
        {
            var viewModel=new SignInAccountViewModel();

            return View(viewModel);
        }

        [HttpPost]
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

            SetAuthCookieAndAddSession(viewModel.Login.ToLower(), true);
            return RedirectToAction("Index", "Player");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private void SetAuthCookieAndAddSession(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

            var rolesList = _accountRoleRepository.GetAccountRoles(userName);
            
            Session.Add("Roles", rolesList);
        }
    }
}