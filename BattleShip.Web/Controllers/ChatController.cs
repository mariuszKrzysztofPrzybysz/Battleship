using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BattleShip.Repository.Interfaces;

namespace BattleShip.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IAccountRepository _repository;

        public ChatController(IAccountRepository repository)
        {
            _repository = repository;
        }

        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
    }
}