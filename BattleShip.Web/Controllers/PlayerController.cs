﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BattleShip.Web.Attributes;

namespace BattleShip.Web.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player
        [BattleShipAuthorize("player")]
        public ActionResult Index()
        {
            return View();
        }
    }
}