using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShip.Database.Entities;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<PlayerBattlesViewModel>> GetPlayerBattles(string login);
    }
}