using System.Collections;
using System.Collections.Generic;
using BattleShip.Database.Entities;

namespace BattleShip.Repository.Interfaces
{
    public interface IAccountRoleRepository
    {
        IEnumerable<Role> GetAccountRoles(string login);
    }
}