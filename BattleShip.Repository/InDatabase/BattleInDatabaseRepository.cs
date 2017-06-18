using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Database;
using BattleShip.Database.Entities;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.InDatabase
{
    public class BattleInDatabaseRepository : IBattleRepository
    {
        private readonly BattleShipContext _context;

        public BattleInDatabaseRepository(BattleShipContext context)
        {
            _context = context;
        }

        public async Task<Result> CreateAsync(CreateBattleViewModel viewModel)
        {
            var player = await _context.Accounts
                .SingleAsync(
                    a => a.Login.Equals(viewModel.PlayerName, StringComparison.OrdinalIgnoreCase));

            var opponent = await _context.Accounts
                .SingleOrDefaultAsync(
                    a => a.Login.Equals(viewModel.OpponentName, StringComparison.OrdinalIgnoreCase));

            if (opponent == null)
                return new Result {IsSuccess = false, ErrorMessage = $"Nie znaleziono gracza {viewModel.OpponentName}"};

            if (player.AccountId == opponent.AccountId)
                return new Result {IsSuccess = false, ErrorMessage = "Nie można rozegrać bitwy z samym sobą"};

            var battle = new Battle
            {
                PlayerId = player.AccountId,
                OpponentId = opponent.AccountId,
                StartUtcDateTime = viewModel.StartUtcDateTime
            };

            var newBattle = _context.Battles.Add(battle);

            if (await _context.SaveChangesAsync() > 0)
                return new Result {IsSuccess = true, Data = new {Id = newBattle.BattleId}};

            return new Result {IsSuccess = false, ErrorMessage = ""};
        }
    }
}