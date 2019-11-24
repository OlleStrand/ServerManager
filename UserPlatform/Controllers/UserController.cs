using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserPlatform.Models;

namespace UserPlatform.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListUsers()
        {
            UserModel _uM = new UserModel();

            return View(_uM.GetUsers());
        }


        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                UserModel _uM = new UserModel();

                _uM.CreateUser(user);
                return RedirectToAction("Index", "Home");
            } else
            {
                return View(user);
            }
        }
    }
}