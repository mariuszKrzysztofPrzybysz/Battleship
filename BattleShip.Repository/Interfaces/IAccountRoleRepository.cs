using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShip.Database.Entities;

namespace BattleShip.Repository.Interfaces
{
    public interface IAccountRoleRepository
    {
        Task<IEnumerable<Role>> GetAccountRolesAsync(string login);
    }
}