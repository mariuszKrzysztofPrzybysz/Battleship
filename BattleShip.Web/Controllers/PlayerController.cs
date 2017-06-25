using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.ViewModels;
using BattleShip.Web.ViewModels;

namespace BattleShip.Web.Controllers
{

    public class PlayerController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public PlayerController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
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

            var model = new ExtendedEditAccountViewModel()
            {
                AccountId = account.AccountId,
                FirstName = account.FirstName,
                LastName = account.LastName,
                EmailAddress = account.EmailAddress,
                Photo = account.Photo,
                AllowNewBattle = account.AllowNewBattle,
                AllowPrivateChat = account.AllowPrivateChat
            };

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

            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync()
        {
            var login = User.Identity.Name;

            var accountInDatabase = await _accountRepository.GetAccountAsync(login);

            if (accountInDatabase == null)
                return RedirectToAction("Index");

            var model = new AccountDetailsViewModel
            {
                Login = accountInDatabase.Login,
                EmailAddress = accountInDatabase.EmailAddress,
                FirstName = accountInDatabase.FirstName,
                LastName = accountInDatabase.LastName,
                Photo = accountInDatabase.Photo,
                Gender = Enum.GetName(typeof(Gender), accountInDatabase.Gender),
                AllowNewBattle = accountInDatabase.AllowNewBattle ? "Yes" : "No",
                AllowPrivateChat = accountInDatabase.AllowPrivateChat ? "Yes" : "No"
            };

            return View(model);
        }
    }
}