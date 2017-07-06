using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
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
            return View();
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

            var result = await _accountRepository.SignUpAsync(viewModel);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(viewModel);
            }

            await SetAuthCookieAndAddSessionAsync(viewModel.Login.ToLower(), true);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [ActionName("SignIn")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignInAsync(SignInAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var authenticatedAccount = await _accountRepository
                .AuthenticateAccountAsync(viewModel.Login, viewModel.Password);

            if (!authenticatedAccount.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, authenticatedAccount.ErrorMessage);
                return View();
            }

            await SetAuthCookieAndAddSessionAsync(viewModel.Login.ToLower(), true);

            return RedirectToAction("Index", "Player");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private async Task SetAuthCookieAndAddSessionAsync(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

            var rolesListInDatabaseForAccount = await _accountRoleRepository.GetAccountRolesAsync(userName);

            Session.Add("Roles", rolesListInDatabaseForAccount);
        }
    }
}