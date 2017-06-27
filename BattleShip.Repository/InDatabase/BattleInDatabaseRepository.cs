using System;
using System.Data.Entity;
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
            try
            {
                var player = await _context.Accounts
                    .SingleAsync(a
                    => a.Login.Equals(viewModel.PlayerName, StringComparison.OrdinalIgnoreCase));

                var opponent = await _context.Accounts
                    .SingleOrDefaultAsync(a
                    => a.Login.Equals(viewModel.OpponentName, StringComparison.OrdinalIgnoreCase));

                if (opponent == null)
                    return new Result {ErrorMessage = $"Nie znaleziono gracza {viewModel.OpponentName}"};

                if (player.AccountId == opponent.AccountId)
                    return new Result {ErrorMessage = "Nie można rozegrać bitwy z samym sobą"};

                var battle = new Battle
                {
                    PlayerId = player.AccountId,
                    OpponentId = opponent.AccountId,
                    StartUtcDateTime = viewModel.StartUtcDateTime,
                    Attacker = opponent.AccountId
                };

                //var newBattle = _context.Battles.Add(battle);

                if (await _context.SaveChangesAsync() > 0)
                    return new Result {IsSuccess = true, Data = new {Id = battle.BattleId}};

                return new Result();
            }
            catch(Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
        }

        public async Task<Result> UploadBoardAsync(long battleId, string userName, string board)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
                    return new Result {ErrorMessage = "Zdefiniuj poprawnie nazwę gracza"};

                if (string.IsNullOrEmpty(board) || string.IsNullOrWhiteSpace(board))
                    return new Result {ErrorMessage = "Uzupełnij planszę"};

                var validatedBoard = new BoardHelper().Validate(board);

                if (!validatedBoard.IsSuccess)
                    return validatedBoard;

                var accountInDatabase =
                    await _context.Accounts.SingleOrDefaultAsync(a
                        => a.Login.Equals(userName, StringComparison.OrdinalIgnoreCase));

                if (accountInDatabase == null)
                    return new Result();

                var battleInDatabase = await _context.Battles
                    .SingleOrDefaultAsync(b => !b.WinnerId.HasValue
                                               && b.BattleId == battleId
                                               &&
                                               (b.Player.Login.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
                                                b.PlayerIsReady == false
                                                ||
                                                b.Opponent.Login.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
                                                b.OpponentIsReady == false));
                if (battleInDatabase == null)
                    return new Result();

                if (!battleInDatabase.PlayerIsReady
                    && accountInDatabase.AccountId == battleInDatabase.PlayerId)
                {
                    battleInDatabase.PlayerBoard = board;
                    battleInDatabase.PlayerIsReady = true;
                }
                if (!battleInDatabase.OpponentIsReady
                    && accountInDatabase.AccountId == battleInDatabase.OpponentId)
                {
                    battleInDatabase.OpponentBoard = board;
                    battleInDatabase.OpponentIsReady = true;
                }

                await _context.SaveChangesAsync();

                return new Result {IsSuccess = true, Data = new {id = battleInDatabase.BattleId}};
            }
            catch (Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
        }

        public async Task<Result> AttackAsync(long battleId, string attackerName, string cell)
        {
            try
            {
                if (string.IsNullOrEmpty(attackerName) || string.IsNullOrWhiteSpace(attackerName))
                    return new Result();

                if (string.IsNullOrEmpty(cell) || string.IsNullOrWhiteSpace(cell))
                    return new Result();

                var battleInDatabase = await _context.Battles
                    .SingleOrDefaultAsync(b => !b.WinnerId.HasValue
                                               && b.BattleId == battleId
                                               &&
                                               (b.Player.Login.Equals(attackerName, StringComparison.OrdinalIgnoreCase)
                                                ||
                                                b.Opponent.Login.Equals(attackerName, StringComparison.OrdinalIgnoreCase)));

                if (battleInDatabase == null)
                    return new Result {ErrorMessage = "Error"};

                string newBoard = String.Empty;
                Result result = null;

                if (battleInDatabase.Player.Login.Equals(attackerName, StringComparison.OrdinalIgnoreCase))
                {
                    newBoard = battleInDatabase.OpponentBoard.Replace(cell, string.Empty);

                    if (newBoard == battleInDatabase.OpponentBoard)
                        result = new Result {IsSuccess = true, Data = new {result = "missed", isGameOver = false}};

                    else if (newBoard == string.Empty)
                    {
                        battleInDatabase.OpponentBoard = newBoard;
                        battleInDatabase.EndUtcDateTime = DateTime.UtcNow;
                        result = new Result
                        {
                            IsSuccess = true,
                            Data =
                                new
                                {
                                    result = "hitted",
                                    isGameOver = true
                                }
                        };
                        battleInDatabase.WinnerId = battleInDatabase.PlayerId;
                    }
                    else
                    {
                        battleInDatabase.OpponentBoard = newBoard;
                        result = new Result {IsSuccess = true, Data = new {result = "hitted"}};
                    }

                    battleInDatabase.Attacker = battleInDatabase.OpponentId;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    newBoard = battleInDatabase.PlayerBoard.Replace(cell, string.Empty);

                    if (newBoard == battleInDatabase.PlayerBoard)
                        result = new Result {IsSuccess = true, Data = new {result = "missed", isGameOver = false}};

                    else if (newBoard == string.Empty)
                    {
                        battleInDatabase.PlayerBoard = newBoard;
                        battleInDatabase.EndUtcDateTime = DateTime.UtcNow;
                        result = new Result
                        {
                            IsSuccess = true,
                            Data =
                                new
                                {
                                    result = "hitted",
                                    isGameOver = true
                                }
                        };
                        battleInDatabase.WinnerId = battleInDatabase.OpponentId;
                    }
                    else
                    {
                        battleInDatabase.PlayerBoard = newBoard;
                        result = new Result {IsSuccess = true, Data = new {result = "hitted"}};
                    }

                    battleInDatabase.Attacker = battleInDatabase.PlayerId;

                    await _context.SaveChangesAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
        }

        public async Task<Result> CheckAccessAsync(long id, string userName)
        {
            try
            {
                var battleInDatabase = await _context.Battles
                    .SingleOrDefaultAsync(b => b.BattleId == id
                                               && (b.Player.Login.Equals(userName, StringComparison.OrdinalIgnoreCase)
                                                   ||
                                                   b.Opponent.Login.Equals(userName, StringComparison.OrdinalIgnoreCase)));

                if (battleInDatabase == null)
                    return new Result();

                if (battleInDatabase.WinnerId != null)
                    return new Result {ErrorMessage = "Bitwa została zakończona"};

                if (battleInDatabase.Player.Login.Equals(userName, StringComparison.OrdinalIgnoreCase))
                    return new Result
                    {
                        IsSuccess = true,
                        Data = new PlayBattleViewModel
                        {
                            PlayerBoard = battleInDatabase.PlayerBoard,
                            Opponent = battleInDatabase.Opponent.Login
                        }
                    };

                if (battleInDatabase.Opponent.Login.Equals(userName, StringComparison.OrdinalIgnoreCase))
                    return new Result
                    {
                        IsSuccess = true,
                        Data = new PlayBattleViewModel
                        {
                            PlayerBoard = battleInDatabase.OpponentBoard,
                            Opponent = battleInDatabase.Player.Login
                        }
                    };

                return new Result {Data = new NotImplementedException()};
            }
            catch (Exception ex)
            {
                return new Result {ErrorMessage = ex.Message};
            }
        }

        public async Task<Result> GiveInAsync(long battleId, string userName)
        {
            var playerInDatabase =
                await _context.Accounts.SingleOrDefaultAsync(a
                    => a.Login.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (playerInDatabase == null)
                return new Result();

            var battleInDatabase = await _context.Battles
                .SingleOrDefaultAsync(b
                    => b.BattleId == battleId
                       && (b.Player.AccountId == playerInDatabase.AccountId
                           || b.Opponent.AccountId == playerInDatabase.AccountId));

            if (battleInDatabase == null)
                return new Result();

            battleInDatabase.WinnerId = battleInDatabase.Player.AccountId == playerInDatabase.AccountId
                ? battleInDatabase.Opponent.AccountId
                : battleInDatabase.Player.AccountId;

            battleInDatabase.EndUtcDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            //var winner = battleInDatabase.Player.Login.Equals(userName, StringComparison.OrdinalIgnoreCase)
            //    ? battleInDatabase.Opponent.Login
            //    : battleInDatabase.Player.Login;

            return new Result {IsSuccess = true};
        }
    }
}