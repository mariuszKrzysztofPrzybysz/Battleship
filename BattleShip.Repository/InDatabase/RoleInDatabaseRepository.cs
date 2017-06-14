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
    public class RoleInDatabaseRepository : IRoleRepository
    {
        private readonly BattleShipContext _context;

        public RoleInDatabaseRepository(BattleShipContext context)
        {
            _context = context;
        }

        IEnumerable<Role> IRoleRepository.GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public byte GetRoleIdOrDefault(string name)
        {
            var result = _context.Roles
                .SingleOrDefault(r => r.Name.Equals(name,
                    StringComparison.OrdinalIgnoreCase));

            if (result != null)
                return result.RoleId;

            return _context.Roles
                .Single(r => r.Name.Equals("Player",
                    StringComparison.OrdinalIgnoreCase)).RoleId;
        }
    }
}