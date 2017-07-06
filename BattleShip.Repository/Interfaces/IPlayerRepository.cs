using System.Collections.Generic;
using System.Threading.Tasks;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<AccountPermissionsViewModel>> GetAllOnlinePlayersExceptAsync(string login);

        Task<IEnumerable<PlayerBattlesViewModel>> GetPlayerBattles(string login);

        Task<AccountPermissionsViewModel> EnterChatWebPage(string accountName);

        Task<Result> ExitChatWebPage(string accountName);
    }
}