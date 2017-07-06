using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Database;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.InDatabase
{
    public class PlayerInDatabaseRepository : IPlayerRepository
    {
        private readonly BattleShipContext _context;

        public PlayerInDatabaseRepository(BattleShipContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlayerBattlesViewModel>> GetPlayerBattles(string login)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrWhiteSpace(login))
                return new List<PlayerBattlesViewModel>();

            var result = await _context.Battles
                .Where(b => b.Player.Login.Equals(login, StringComparison.OrdinalIgnoreCase)
                            || b.Opponent.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                .Select(b => new PlayerBattlesViewModel
                {
                    BattleId = b.BattleId,
                    Player = b.Player.Login,
                    Opponent = b.Opponent.Login,
                    StartUtcDateTime = b.StartUtcDateTime,
                    Duration = b.EndUtcDateTime.HasValue
                        ? DbFunctions.DiffSeconds(b.StartUtcDateTime, b.EndUtcDateTime)
                        : DbFunctions.DiffSeconds(b.StartUtcDateTime, DateTime.UtcNow),
                    Winner = b.WinnerId.HasValue
                        ? (b.WinnerId == b.Player.AccountId ? b.Player.Login : b.Opponent.Login)
                        : string.Empty
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<AccountPermissionsViewModel>> GetAllOnlinePlayersExceptAsync(string login)
        {
            try
            {
                var result = await _context.Accounts
                    .Where(a => a.IsOnChatWebPage
                                && !a.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                    .Select(a => new AccountPermissionsViewModel
                    {
                        Login = a.Login,
                        AllowPrivateChat = a.AllowPrivateChat,
                        AllowNewBattle = a.AllowNewBattle
                    })
                    .ToListAsync();

                return result;
            }
            catch
            {
                return new List<AccountPermissionsViewModel>();
            }
        }

        public async Task<AccountPermissionsViewModel> EnterChatWebPage(string accountName)
        {
            try
            {
                var oldAccountInDatabase = await _context.Accounts.SingleAsync(a =>
                    a.Login.Equals(accountName, StringComparison.OrdinalIgnoreCase));

                oldAccountInDatabase.IsOnChatWebPage = true;

                await _context.SaveChangesAsync();

                var account =
                    await _context.Accounts.SingleOrDefaultAsync(
                        a => a.Login.Equals(accountName, StringComparison.OrdinalIgnoreCase));

                if (account != null)
                    return new AccountPermissionsViewModel
                    {
                        Login = account.Login,
                        AllowNewBattle = account.AllowNewBattle,
                        AllowPrivateChat = account.AllowPrivateChat
                    };

                return new AccountPermissionsViewModel();
            }
            catch
            {
                return new AccountPermissionsViewModel();
            }
        }

        public async Task<Result> ExitChatWebPage(string accountName)
        {
            try
            {
                var oldAccountInDatabase = _context.Accounts.Single(a =>
                    a.Login.Equals(accountName, StringComparison.OrdinalIgnoreCase));

                oldAccountInDatabase.IsOnChatWebPage = false;

                await _context.SaveChangesAsync();

                return new Result {IsSuccess = true};
            }
            catch (Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
        }
    }
}