﻿using Microsoft.AspNetCore.Mvc;

namespace GedsiHub.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
