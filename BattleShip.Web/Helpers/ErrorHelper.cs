using System.Web.Mvc;
using System.Web.Routing;

namespace BattleShip.Web.Helpers
{
    public static class ErrorHelper
    {
        public static RedirectToRouteResult RedirectToError(string message)
        {
            var routeValues = new RouteValueDictionary
            {
                {"Controller", "Errors"},
                {"Action", "Index"},
                {"message", message}
            };

            return new RedirectToRouteResult(routeValues);
        }
    }
}