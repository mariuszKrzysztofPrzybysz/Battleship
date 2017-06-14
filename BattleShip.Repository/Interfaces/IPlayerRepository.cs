using System.Security.Cryptography.X509Certificates;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Interfaces
{
    public interface IPlayerRepository
    {
        Result Add(AddAccountViewModel viewModel);
    }
}