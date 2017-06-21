using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IBattleRepository
    {
        Task<Result> CreateAsync(CreateBattleViewModel battle);

        Task<Result> UploadBoardAsync(long battleId, string userName, string board);

        Task<Result> AttackAsync(long battleId, string attackerName, string cell);
        Task<Result> CheckAccessAsync(long id, string userName);
    }
}