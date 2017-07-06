using System.Web.Mvc;

namespace BattleShip.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string message)
        {
            ViewBag.ErrorMessage = message;

            return View();
        }
    }
}