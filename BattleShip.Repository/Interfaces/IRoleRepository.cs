using System.Collections;
using System.Collections.Generic;
using BattleShip.Database.Entities;
using BattleShip.Repository.RepositoryHelpers;

namespace BattleShip.Repository.Interfaces
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAllRoles();
        byte GetRoleIdOrDefault(string name);
    }
}