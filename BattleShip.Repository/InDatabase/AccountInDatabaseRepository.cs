﻿using System;
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
    }
}