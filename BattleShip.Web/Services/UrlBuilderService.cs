using System.Web;

namespace BattleShip.Web.Services
{
    public static class UrlBuilderService
    {
        private static readonly string Scheme = HttpContext.Current.Request.Url.Scheme;
        private static readonly string Authority = HttpContext.Current.Request.Url.Authority;

        public static string GetUrl(string controller, string action)
        {
            return $"{Scheme}://{Authority}/{controller}/{action}";
        }

        public static string GetUrl(string controller, string action, string[] queryStrings)
        {
            var result = GetUrl(controller, action);

            if (queryStrings != null)
            {
                if (queryStrings.Length > 0)
                    result += $"?{queryStrings[0]}";
                for (byte i = 1; i < queryStrings.Length; i++)
                    result += $"&{queryStrings[i]}";
            }

            return result;
        }
    }
}