using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Result Add(AddAccountViewModel viewModel);

        Result AuthenticateAccount(string login, string password);

        IEnumerable<OnChatWebPageViewModel> GetOnlinePlayersExcept(string login);
    }
}