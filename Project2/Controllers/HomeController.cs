using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Project2.Models;

namespace Project2.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginForm form)
        {
            if (ModelState.IsValid)
            {
                Tuple<int, string> idRole = await DatabaseOperations.Login(form);

                int UserID = idRole.Item1;
                string UserRole = idRole.Item2;

                if (UserID != 0)
                {
                    if (UserRole == "Admin")
                    {
                        HttpContext.Session.SetInt32("adminID", UserID);
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (UserRole == "User")
                    {
                        HttpContext.Session.SetInt32("userID", UserID);
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return View(form);
                }

            }
            return View(form);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterForm form)
        {
            if (ModelState.IsValid)
            {
                await DatabaseOperations.Register(form);

                await Email.RegisterEmail(form);

                return RedirectToAction("Index", "Home");
            }
            return View(form);
        }

    }
}
