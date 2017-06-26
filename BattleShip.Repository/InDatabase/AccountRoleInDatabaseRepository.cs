using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Role>> GetAccountRolesAsync(string login)
        {
            return await _context.Roles
                .Where(r =>
                    r.AccountRoles.Any(ar =>
                        ar.Account.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                .ToListAsync();
        }
    }
}