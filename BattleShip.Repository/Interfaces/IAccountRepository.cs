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
        Task<Result> SignUpAsync(AddAccountViewModel viewModel);

        Task<Result> AuthenticateAccountAsync(string login, string password);

        Task<Result> UpdateAccountAsync(EditAccountViewModel viewModel);

        Task<Account> GetAccountAsync(string login);
    }
}