using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServerManager.Controllers
{
    public class PricingController : Controller
    {
        // GET: Pricing
        public ActionResult Index() => View();

        public ActionResult Basic() => View();
        public ActionResult Professional() => View();
        public ActionResult Business() => View();
    }
}