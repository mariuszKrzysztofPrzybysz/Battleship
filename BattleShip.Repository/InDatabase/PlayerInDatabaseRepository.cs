using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Database;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;
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
            if(string.IsNullOrEmpty(login) || string.IsNullOrWhiteSpace(login))
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
                    ? (b.WinnerId==b.Player.AccountId ? b.Player.Login : b.Opponent.Login)
                    : string.Empty
                })
                .ToListAsync();

            return result;
        }
    }
}
