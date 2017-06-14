using System.Web;
using System.Web.Mvc;
using BattleShip.Web.Attributes;

namespace BattleShip.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
            filters.Add(new BattleShipAuthorizeAttribute());
        }
    }
}
