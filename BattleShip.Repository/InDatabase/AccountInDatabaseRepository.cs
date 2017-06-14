using System;
using System.Collections.Generic;
using System.Linq;
using BattleShip.Database;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.InDatabase
{
    public class AccountInDatabaseRepository : IAccountRepository
    {
        private readonly BattleShipContext _context;

        public AccountInDatabaseRepository(BattleShipContext context)
        {
            _context = context;
        }

        public Result Add(AddAccountViewModel viewModel)
        {
            var isExistsAccountInDatabase = _context.Accounts
                .Any(a => a.Login.Equals(viewModel.Login, StringComparison.OrdinalIgnoreCase));

            if (isExistsAccountInDatabase)
                return new Result
                {
                    IsSuccess = false,
                    ErrorMessage =
                        $"Ta nazwa użytkownika jest już zajęta. " +
                        $"Pamiętaj, że w nazwach użytkowników ignorujemy kropki " +
                        $"i nie rozróżniamy wielkości liter. Chcesz podać inną nazwę?"
                };

            var hashedPassword = PasswordHelper.GetSha512CngPasswordHash(viewModel.Password);

            var account = new Account
            {
                Login = viewModel.Login,
                Password = hashedPassword,
                EmailAddress = viewModel.EmailAddress,
                FirstName = viewModel.FirstName ?? string.Empty,
                LastName = viewModel.LastName ?? string.Empty,
                Gender = viewModel.Gender,
                AllowNewBattle = viewModel.AllowNewBattle,
                AllowPrivateChat = viewModel.AllowPrivateChat
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();
            var accountId = account.AccountId;

            if (accountId <= 0)
                return new Result
                {
                    IsSuccess = false,
                    ErrorMessage = "Nie utworzono konta użytkownika"
                };

            //TODO: Replace magic string with ...
            var playerRoleId = _context.Roles
                .Single(r => r.Name.Equals("Player",
                    StringComparison.OrdinalIgnoreCase)).RoleId;

            var accountRole = new AccountRole
            {
                AccountId = accountId,
                RoleId = playerRoleId
            };

            _context.AccountRoles.Add(accountRole);
            _context.SaveChanges();
            var accountRoleId = accountRole.AccountRoleId;

            if (accountRoleId > 0)
                return new Result {IsSuccess = true};

            return new Result
            {
                IsSuccess = false,
                ErrorMessage = "Nie utworzono roli dla użytkownika"
            };
        }

        public Result AuthenticateAccount(string login, string password)
        {
            var hashedPassword = PasswordHelper.GetSha512CngPasswordHash(password);

            var account = _context.Accounts
                .SingleOrDefault(p =>
                    p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)
                    && p.Password == hashedPassword);

            if (account == null)
                return new Result {IsSuccess = false, ErrorMessage = "Wprowadzony adres e-mail lub hasło nie pasuje do żadnego konta" };

            return new Result {IsSuccess = true};
        }

        public IEnumerable<OnChatWebPageViewModel> GetOnlinePlayersExcept(string login)
        {
            return _context.Accounts
                .Where(a => a.IsOnChatWebPage == true
                            && !a.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                .Select(a => new OnChatWebPageViewModel
                {
                    Login = a.Login,
                    AllowPrivateChat = a.AllowPrivateChat,
                    AllowNewBattle = a.AllowNewBattle
                })
                .ToList();
        }
    }
}