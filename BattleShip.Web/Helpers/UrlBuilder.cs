using System.Web;

namespace BattleShip.Web.Helpers
{
    public static class UrlBuilder
    {
        private static readonly string Scheme = HttpContext.Current.Request.Url.Scheme;
        private static readonly string Authority = HttpContext.Current.Request.Url.Authority;

        private static string GetUrl(string controller, string action)
        {
            return $"{Scheme}://{Authority}/{controller}/{action}";
        }

        public static string GetUrl(string controller, string action, string[] queries)
        {
            var result = GetUrl(controller, action);

            if (queries == null)
                return result;

            if (queries.Length > 0)
                result += $"?{queries[0]}";

            for (byte i = 1; i < queries.Length; i++)
                result += $"&{queries[i]}";

            return result;
        }
    }
}