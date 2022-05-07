using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Project2.Models;
using System;

namespace Project2.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.user = userID;

            return View(userID);
        }

        public IActionResult Logout()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            this.HttpContext.Session.Remove("userID");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Motorbikes()
        {
            List<Motorbike> motorbikes = await DatabaseOperations.AllMotorbikes();

            ViewBag.motorbikes = motorbikes;

            return View();
        }

        public async Task<IActionResult> MotoDetail(int id)
        {
            Motorbike motorbike = await DatabaseOperations.SelectMotorbike(id);

            ViewBag.motorbike = motorbike;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MotoDetail(int id, Reservation form)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            form.MotorbikeId = id;
            form.userId = userID;

            if (ModelState.IsValid)
            {
                await DatabaseOperations.NewReservation(form, userID);
                return RedirectToAction("Order", "User");
            }

            return RedirectToAction("MotoDetail", "User", new { id = id });
        }


        [HttpGet]
        public async Task<IActionResult> Servis(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Office> offices = await DatabaseOperations.AllOffices();

            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (Office office in offices)
            {
                selectList.Add(new SelectListItem
                {
                    Text = office.City,
                    Value = office.City
                });
            }

            ViewBag.offices = selectList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Servis(int id, Servis servis)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                await DatabaseOperations.NewServis(servis, userID);

                return RedirectToAction("Index", "User", new { id = id });
            }
            
            return View(servis);
        }


        public async Task<IActionResult> Order()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Order> userOrders = await DatabaseOperations.UserOrder(userID);

            ViewBag.userOrders = userOrders;

            userOrders.Count();

            return View();
        }


        public async Task<IActionResult> Dismiss(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            await DatabaseOperations.RemoveOrder(id);

            return RedirectToAction("Order", "User");
        }
        

        public async Task<IActionResult> PayAll()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            await DatabaseOperations.PayAllOrders(userID);

            return RedirectToAction("Order", "User");
        }



        public async Task AddMotorbikeOption()
        {
            List<Motorbike> motorbikes = new List<Motorbike>();

            motorbikes = await DatabaseOperations.AllMotorbikes();

            ViewBag.motorbikes = motorbikes;

            List<SelectListItem> SelectMotorbikes = new List<SelectListItem>();

            foreach (Motorbike motorbike in motorbikes)
            {
                SelectMotorbikes.Add(new SelectListItem { Text = motorbike.Name, Value = motorbike.Id.ToString() });
            }

            ViewBag.SelectMotorbikes = SelectMotorbikes;
        }

        [HttpGet]
        public async Task<IActionResult> Reservation()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            await AddMotorbikeOption();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reservation(Reservation form)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            await AddMotorbikeOption();
            
            if(ModelState.IsValid)
            {
                await DatabaseOperations.NewReservation(form, userID);
                return RedirectToAction("Index", "User");
            }    
            
            return View();
        }

    }
}
