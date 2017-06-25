using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BattleShip.Database.Entities;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<Result> Add(AddAccountViewModel viewModel);

        Task<Result> AuthenticateAccount(string login, string password);

        Task<IEnumerable<AccountPermissionsViewModel>> GetOnlinePlayersExcept(string login);

        Task<AccountPermissionsViewModel> EnterChatWebPage(string accountName);

        Task<Result> ExitChatWebPage(string accountName);

        Task<Result> UpdateAccountAsync(EditAccountViewModel viewModel);

        Task<Account> GetAccountAsync(string login);
    }
}