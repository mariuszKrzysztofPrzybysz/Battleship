using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BattleShip.Database;
using BattleShip.Repository.InDatabase;
using BattleShip.Repository.Interfaces;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Web.Controllers.Api
{
    public class BattlesController : ApiController
    {
        private readonly IPlayerRepository _playerRepository;

        public BattlesController()
        {
            //TODO: Implement Castle Windsor
            _playerRepository = new PlayerInDatabaseRepository(new BattleShipContext());
        }

        // GET /api/battles
        public async Task<IEnumerable<PlayerBattlesViewModel>> GetBattles()
        {
            var login = User.Identity.Name;

            return await _playerRepository.GetPlayerBattles(login);
        }
    }
}
