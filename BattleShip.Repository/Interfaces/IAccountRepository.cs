using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Result Add(AddAccountViewModel viewModel);

        Result AuthenticateAccount(string login, string password);

        Task<IEnumerable<AccountPermissionsViewModel>> GetOnlinePlayersExcept(string login);

        Task<AccountPermissionsViewModel> EnterChatWebPage(string accountName);

        Task<Result> ExitChatWebPage(string accountName);
    }
}