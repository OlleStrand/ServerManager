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

        public ActionResult Login()
        {
            return View();
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

                if (user.Password.Length < 8)
                {
                    if (user.Password.Length > 20)
                    {
                        ModelState.AddModelError("password", "Password too long! (Max 20 characters)");
                        user.Password = string.Empty;
                        return View(user);
                    }
                    else
                    {
                        ModelState.AddModelError("password", "Password too short! (Min 8 characters)");
                        user.Password = string.Empty;
                        return View(user);
                    }
                }

                if (_uM.CreateUser(user))
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
    }
}