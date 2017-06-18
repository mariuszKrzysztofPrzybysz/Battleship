using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IBattleRepository
    {
        Task<Result> CreateAsync(CreateBattleViewModel battle);
    }
}