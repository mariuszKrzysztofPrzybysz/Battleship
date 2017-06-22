using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BattleShip.Web.Hubs
{
    public class BattleHub : Hub
    {
        public async Task JoinBattle(long battleId)
        {
            await Groups.Add(Context.ConnectionId, battleId.ToString());
        }

        public async Task LeaveBattle(long battleId)
        {
            await Groups.Remove(Context.ConnectionId, battleId.ToString());
        }

        public void GiveIn(long battleId, string targetLocation)
        {
            var opponent = HttpContext.Current.User.Identity.Name;

            Clients.OthersInGroup(battleId.ToString()).opponentHasGivenIn(opponent, targetLocation);
        }
    }
}