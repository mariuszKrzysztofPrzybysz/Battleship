using System;
using System.Collections.Generic;
using System.Linq;
using BattleShip.Database;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;

namespace BattleShip.Repository.InDatabase
{
    public class AccountRoleInDatabaseRepository : IAccountRoleRepository
    {
        private readonly BattleShipContext _context;

        public AccountRoleInDatabaseRepository(BattleShipContext context)
        {
            _context = context;
        }

        public IEnumerable<Role> GetAccountRoles(string login)
        {
            return _context.Roles
                .Where(r =>
                    r.AccountRoles.Any(ar =>
                        ar.Account.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
    }
}