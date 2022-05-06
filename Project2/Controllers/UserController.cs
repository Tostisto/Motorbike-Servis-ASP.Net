using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Project2.Models;

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


        [HttpGet]
        public IActionResult Servis(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

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
