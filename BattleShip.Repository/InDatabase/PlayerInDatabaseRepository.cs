using System;
using BattleShip.Database;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.InDatabase
{
    public class PlayerInDatabaseRepository : IPlayerRepository
    {
        private readonly BattleShipContext _context;

        public PlayerInDatabaseRepository(BattleShipContext context)
        {
            _context = context;
        }

        public Result Add(AddAccountViewModel viewModel)
        {
            var hashedPassword = PasswordHelper.GetSha512CngPasswordHash(viewModel.Password);

            var account = new Account
            {
                Login = viewModel.Login,
                Password = hashedPassword,
                EmailAddress = viewModel.EmailAddress,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Gender = viewModel.Gender,
                AllowNewBattle = viewModel.AllowNewBattle,
                AllowPrivateChat = viewModel.AllowPrivateChat
            };

            _context.Accounts.Add(account);
            var accountId = _context.SaveChanges();

            if (accountId <= 0)
                return new Result
                {
                    IsSuccess = false,
                    ErrorMessage = "Nie utworzono konta użytkownika"
                };
            //TODO: Pobrać IdRole dla 'Player'

            var accountRole = new AccountRole
            {
                AccountId = accountId,
                RoleId = 1 //TODO: = roleId
            };

            _context.AccountRoles.Add(accountRole);
            var accountRoleId = _context.SaveChanges();

            if (accountRoleId > 0)
                return new Result {IsSuccess = true};

            return new Result
            {
                IsSuccess = false,
                ErrorMessage = "Nie utworzono roli dla użytkownika"
            };
        }
    }
}