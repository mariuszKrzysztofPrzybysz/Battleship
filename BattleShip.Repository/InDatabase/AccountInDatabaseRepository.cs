using System;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BattleShip.Database;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.Profiles;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.InDatabase
{
    public class AccountInDatabaseRepository : IAccountRepository
    {
        private readonly BattleShipContext _context;

        static AccountInDatabaseRepository()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new AccountProfile()));
        }

        public AccountInDatabaseRepository(BattleShipContext context)
        {
            _context = context;
        }

        public async Task<Result> SignUpAsync(AddAccountViewModel viewModel)
        {
            try
            {
                using (var transacion = _context.Database.BeginTransaction())
                {
                    try
                    {
                        const string defaultRoleForNewAccount = "player";

                        var isAccountExistsInDatabase = await _context.Accounts
                            .AnyAsync(a => a.Login.Equals(viewModel.Login, StringComparison.OrdinalIgnoreCase));

                        if (isAccountExistsInDatabase)
                            return new Result
                            {
                                ErrorMessage =
                                    $"Ta nazwa użytkownika jest już zajęta. " +
                                    $"Pamiętaj, że w nazwach użytkowników ignorujemy kropki " +
                                    $"i nie rozróżniamy wielkości liter. Chcesz podać inną nazwę?"
                            };

                        

                        var newAccount = Mapper.Map<AddAccountViewModel, Account>(viewModel);
                        
                        _context.Accounts.Add(newAccount);

                        var numberOfAccountsWrittenToUnderlyingDatabase
                            = await _context.SaveChangesAsync();

                        if (numberOfAccountsWrittenToUnderlyingDatabase == 0)
                            return new Result {ErrorMessage = "Nie utworzono konta użytkownika"};

                        var defaultPlayerRoleForNewAccountInDatabase
                            = await _context.Roles.SingleAsync(r
                                => r.Name.Equals(defaultRoleForNewAccount, StringComparison.OrdinalIgnoreCase));

                        var newAccountRole = new AccountRole
                        {
                            AccountId = newAccount.AccountId,
                            RoleId = defaultPlayerRoleForNewAccountInDatabase.RoleId
                        };

                        _context.AccountRoles.Add(newAccountRole);

                        var numberOfAccountRolesWrittenToUnderlyingDatabase
                            = await _context.SaveChangesAsync();

                        if (numberOfAccountRolesWrittenToUnderlyingDatabase == 0)
                        {
                            transacion.Rollback();
                            return new Result
                            {
                                ErrorMessage =
                                    "Nie znaleziono domyślnej roli dla użytkownika. Skontaktuj się z administratorem"
                            };
                        }

                        transacion.Commit();
                        return new Result {IsSuccess = true};
                    }
                    catch
                    {
                        transacion.Rollback();
                        return new Result {ErrorMessage = "Nie utworzono konta użytkownika"};
                    }
                }
            }
            catch(Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
        }

        public async Task<Result> AuthenticateAccountAsync(string login, string password)
        {
            var hashedPassword = PasswordHelper.GetSha512CngPasswordHash(password);

            try
            {
                var accountInDatabase = await _context.Accounts
                    .SingleOrDefaultAsync(p =>
                        p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)
                        && p.Password == hashedPassword);

                return accountInDatabase == null
                    ? new Result
                    {
                        ErrorMessage = "Wprowadzony adres e-mail lub hasło nie pasuje do żadnego konta"
                    }
                    : new Result {IsSuccess = true};
            }
            catch(Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
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

                Mapper.Map(viewModel, accountInDatabase);

                if (viewModel.Photo != null)
                    accountInDatabase.Photo = viewModel.Photo;

                await _context.SaveChangesAsync();

                return new Result {IsSuccess = true};
            }
            catch (Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
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
            catch
            {
                return null;
            }
        }
    }
}