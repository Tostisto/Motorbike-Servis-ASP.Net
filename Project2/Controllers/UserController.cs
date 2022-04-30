using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Project2.Models;

namespace Project2.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index(int id)
        {

            ViewBag.user = id;

            return View(id);
        }


        [HttpGet]
        public IActionResult Servis(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Servis(int id, Servis servis)
        {
            if (ModelState.IsValid)
            {
                using(SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
                {
                    using(SqliteCommand cmd = new SqliteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Services (UserID, Brand, Model, Year, SPZ, Shop, Description, Status) VALUES (@UserID, @Brand, @Model, @Year, @SPZ, @Shop, @Description, @Status)";
                        cmd.Parameters.AddWithValue("@UserID", id);
                        cmd.Parameters.AddWithValue("@Brand", servis.Brand);
                        cmd.Parameters.AddWithValue("@Model", servis.Model);
                        cmd.Parameters.AddWithValue("@Year", servis.Year);
                        cmd.Parameters.AddWithValue("@SPZ", servis.SPZ);
                        cmd.Parameters.AddWithValue("@Shop", servis.Shop);
                        cmd.Parameters.AddWithValue("@Description", servis.Description);
                        cmd.Parameters.AddWithValue("@Status", "new");
                        
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                return RedirectToAction("Index", "User", new { id = id });

            }
            return View(servis);
        }


        public IActionResult Order(int id)
        {
            return View(id);
        }

        public IActionResult Reservation(int id)
        {
            return View(id);
        }


    }
}
