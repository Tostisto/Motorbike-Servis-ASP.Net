using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Project2.Models;

namespace Project2.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = userID;

            return View(userID);
        }

        public IActionResult LogOut()
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            this.HttpContext.Session.Remove("adminID");

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Motorbikes()
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Motorbikes = await DatabaseOperations.AllMotorbikes();

            return View();
        }


        [HttpGet]
        public IActionResult NewMotorbike()
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Motorbikes", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewMotorbike(Motorbike newMotorbike)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                await DatabaseOperations.NewMotorbike(newMotorbike);

                return RedirectToAction("Index");
            }
            return View(newMotorbike);
        }

        [HttpGet]
        public async Task<IActionResult> ServisManage()
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Servis> servises = await DatabaseOperations.AllServis();

            ViewBag.servises = servises;

            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var servisDetail = await DatabaseOperations.ServisDetail(id);

            ViewBag.servis = servisDetail.Item1;
            ViewBag.user = servisDetail.Item2;

            return View();
        }

        public async Task<IActionResult> Accept(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            await DatabaseOperations.ServisStatus(id, "accepted");

            return RedirectToAction("ServisManage");
        }

        
        public async Task<IActionResult> Dismiss(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            await DatabaseOperations.ServisStatus(id, "dismissed");

            return RedirectToAction("ServisManage");
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.motorbike = await DatabaseOperations.SelectMotorbike(id);

            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Motorbike form)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            form.Id = id;

            if (ModelState.IsValid)
            {
                await DatabaseOperations.EditMotorbike(form);
                return RedirectToAction("Motorbikes");
            }
                
            return View();
        }

        
        public async Task<IActionResult> Remove(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            await DatabaseOperations.RemoveMotorbike(id);   

            return RedirectToAction("Motorbikes");
        }

        
        public async Task<IActionResult> MotorbikeDetail(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
