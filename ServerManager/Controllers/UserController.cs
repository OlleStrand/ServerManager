using System;
using System.Web.Mvc;
using ServerManager.Models;

namespace ServerManager.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index() => View();
        public ActionResult ListUsers() => View(new UserModel().GetUsers());

        public ActionResult Edit(int Id) => View(new UserModel().GetUserById(Id));
        [HttpPost]
        public ActionResult Edit(UserModel user)
        {
            user.UpdateUser();
            
            return RedirectToAction("ListUsers");
        }

        public ActionResult CreateUser() => View();
        [HttpPost]
        public ActionResult CreateUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (user.Password == null)
                {
                    ModelState.AddModelError("password", "No Password");
                    user.Password = string.Empty;
                    return View(user);
                }

                if (user.Password.Length < 8)
                {
                    ModelState.AddModelError("password", "Password too short! (Min 8 characters)");
                    user.Password = string.Empty;
                    return View(user);
                }

                if (user.Password.Length > 20)
                {
                    ModelState.AddModelError("password", "Password too long! (Max 20 characters)");
                    user.Password = string.Empty;
                    return View(user);
                }

                if (user.CreateUser())
                {
                    return RedirectToAction("ListUsers", "User");
                }
                else
                {
                    ModelState.AddModelError("username", "The username is already in use!");
                    user.Password = string.Empty;
                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }

        public ActionResult Login() => View();
        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            if (user.Password == null || user.Username == null)
            {
                ModelState.AddModelError("noCredentials", "No Password or Username!");
                return View(user);
            }

            if (user.Login())
            {
                user = user.GetUser();
                Session["UserID"] = user.UserID;
                Session["UserIsAdmin"] = user.AdminLevel > 0 ? "1" : "0";
                return RedirectToAction("Dashboard");
            }
            else
            {
                ModelState.AddModelError("noCredentials", "Wrong Password or Username!");
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Contents.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Dashboard() => View(new UserModel().GetUserById(Convert.ToInt32(Session["UserID"])));
    }
}