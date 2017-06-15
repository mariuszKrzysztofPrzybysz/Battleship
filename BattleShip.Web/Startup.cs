using Microsoft.Owin;
using Owin;
using BattleShip.Web;

[assembly: OwinStartup(typeof(BattleShip.Web.Startup))]
namespace BattleShip.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}