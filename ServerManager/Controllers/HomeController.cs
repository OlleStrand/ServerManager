﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServerManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() => View();

        public ActionResult About() => View();

        public ActionResult Contact()
        {
            return View();
        }
    }
}