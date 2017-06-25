using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Result> Add(AddAccountViewModel viewModel)
        {
            var isExistsAccountInDatabase = await _context.Accounts
                .AnyAsync(a => a.Login.Equals(viewModel.Login, StringComparison.OrdinalIgnoreCase));

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
                AllowNewBattle = true,
                AllowPrivateChat = true
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            var accountId = account.AccountId;

            if (accountId <= 0)
                return new Result
                {
                    IsSuccess = false,
                    ErrorMessage = "Nie utworzono konta użytkownika"
                };

            //TODO: Replace magic string with ...
            var playerRole = await _context.Roles
                .SingleAsync(r => r.Name.Equals("Player",
                    StringComparison.OrdinalIgnoreCase));

            var playerRoleId = playerRole.RoleId;

            var accountRole = new AccountRole
            {
                AccountId = accountId,
                RoleId = playerRoleId
            };

            _context.AccountRoles.Add(accountRole);
            await _context.SaveChangesAsync();
            var accountRoleId = accountRole.AccountRoleId;

            if (accountRoleId > 0)
                return new Result {IsSuccess = true};

            return new Result
            {
                IsSuccess = false,
                ErrorMessage = "Nie utworzono roli dla użytkownika"
            };
        }

        public async Task<Result> AuthenticateAccount(string login, string password)
        {
            var hashedPassword = PasswordHelper.GetSha512CngPasswordHash(password);

            var account = await _context.Accounts
                .SingleOrDefaultAsync(p =>
                    p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)
                    && p.Password == hashedPassword);

            return account == null
                ? new Result
                {
                    IsSuccess = false,
                    ErrorMessage = "Wprowadzony adres e-mail lub hasło nie pasuje do żadnego konta"
                }
                : new Result {IsSuccess = true};
        }

        public async Task<IEnumerable<AccountPermissionsViewModel>> GetOnlinePlayersExcept(string login)
        {            
            var result = await _context.Accounts
                .Where(a => a.IsOnChatWebPage
                            && !a.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                .Select(a => new AccountPermissionsViewModel
                {
                    Login = a.Login,
                    AllowPrivateChat = a.AllowPrivateChat,
                    AllowNewBattle = a.AllowNewBattle
                })
                .ToListAsync();

            return  result;
        }

        public async Task<AccountPermissionsViewModel> EnterChatWebPage(string accountName)
        {
            try
            {
                var oldAccountInDatabase = await _context.Accounts.SingleAsync(a =>
                    a.Login.Equals(accountName, StringComparison.OrdinalIgnoreCase));

                oldAccountInDatabase.IsOnChatWebPage = true;

                await _context.SaveChangesAsync();

                var account =
                    await _context.Accounts.SingleOrDefaultAsync(
                        a => a.Login.Equals(accountName, StringComparison.OrdinalIgnoreCase));

                if (account != null)
                    return new AccountPermissionsViewModel
                    {
                        Login = account.Login,
                        AllowNewBattle = account.AllowNewBattle,
                        AllowPrivateChat = account.AllowPrivateChat
                    };

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Result> ExitChatWebPage(string accountName)
        {
            try
            {
                var oldAccountInDatabase = _context.Accounts.Single(a =>
                    a.Login.Equals(accountName, StringComparison.OrdinalIgnoreCase));

                oldAccountInDatabase.IsOnChatWebPage = false;

                await _context.SaveChangesAsync();

                return new Result {IsSuccess = true};
            }
            catch (Exception ex)
            {
                return new Result {IsSuccess = false, ErrorMessage = ex.Message};
            }
        }

        public async Task<Result> UpdateAccountAsync(EditAccountViewModel viewModel)
        {
            try
            {
                var accountInDatabase = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.AccountId == viewModel.AccountId);

                if (accountInDatabase == null)
                    return new Result {ErrorMessage = "Skontaktuj się z administratorem."};

                accountInDatabase.EmailAddress = viewModel.EmailAddress;
                accountInDatabase.FirstName = viewModel.FirstName;
                accountInDatabase.LastName = viewModel.LastName;
                accountInDatabase.AllowNewBattle = viewModel.AllowNewBattle;
                accountInDatabase.AllowPrivateChat = viewModel.AllowPrivateChat;
                accountInDatabase.Photo = viewModel.Photo;

                await _context.SaveChangesAsync();

                return new Result {IsSuccess = true};
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
            catch (DbEntityValidationException ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
            catch (DbUpdateException ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
            catch (NotSupportedException ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
            catch (ObjectDisposedException ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
            catch (InvalidOperationException ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
            catch (Exception)
            {
                return new Result
                {
                    ErrorMessage =
                        "Wystąpił błąd związany z siecią lub wystąpieniem podczas ustanawiania połączenia z serwerem programu SQL Server. Nie można odnaleźć serwera lub jest on niedostępny."
                };
            }
        }

        public async Task<Account> GetAccountAsync(string login)
        {
            try
            {
                var accountInDatabase = await _context.Accounts
                    .SingleOrDefaultAsync(a =>
                        a.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

                return accountInDatabase;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}