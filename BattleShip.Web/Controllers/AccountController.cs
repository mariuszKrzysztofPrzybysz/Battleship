using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;

        public AccountController(IAccountRepository repository, IAccountRoleRepository accountRoleRepository)
        {
            _accountRepository = repository;
            _accountRoleRepository = accountRoleRepository;
        }

        // GET: Account
        public ActionResult SignUp()
        {
            var viewModel = new SignUpAccountViewModel();

            return View(viewModel);
        }

        [ActionName("SignUp")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUpAsync(SignUpAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Password = string.Empty;
                viewModel.ConfirmPassword = string.Empty;

                return View(viewModel);
            }

            var result = await _accountRepository.RegisterAsync(viewModel);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(viewModel);
            }

            await SetAuthCookieAndAddSession(viewModel.Login.ToLower(), true);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignIn()
        {
            var viewModel=new SignInAccountViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(SignInAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var authenticatedAccount = await _accountRepository
                .AuthenticateAccount(viewModel.Login, viewModel.Password);

            if (!authenticatedAccount.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, authenticatedAccount.ErrorMessage);
                return View();
            }

            await SetAuthCookieAndAddSession(viewModel.Login.ToLower(), true);
            return RedirectToAction("Index", "Player");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private async Task SetAuthCookieAndAddSession(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

            var rolesList = await _accountRoleRepository.GetAccountRoles(userName);
            
            Session.Add("Roles", rolesList);
        }
    }
}