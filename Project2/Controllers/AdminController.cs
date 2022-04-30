using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Project2.Models;

namespace Project2.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index(int id)
        {
            ViewBag.user = id;

            return View(id);
        }


        [HttpGet]
        public IActionResult NewMotorbike(int id)
        {
            ViewBag.user = id;

            return View();
        }

        [HttpPost]
        public IActionResult NewMotorbike(int id, NewMotorbike newMotorbike)
        {
            ViewBag.user = id;

            if (ModelState.IsValid)
            {
                using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
                {
                    conn.Open();

                    using(SqliteCommand cmd = new SqliteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Motorbike (Name, Price, Description, Image) VALUES (@Name, @Price, @Description, @Image)";

                        cmd.Parameters.AddWithValue("@Name", newMotorbike.Name);
                        cmd.Parameters.AddWithValue("@Price", newMotorbike.Price);
                        cmd.Parameters.AddWithValue("@Description", newMotorbike.Description);
                        if(newMotorbike.Image == "" || newMotorbike.Image == null)
                        {
                            cmd.Parameters.AddWithValue("@Image", DBNull.Value);
                        }   
                        else
                        {
                            cmd.Parameters.AddWithValue("@Image", newMotorbike.Image);
                        }
                        
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                return RedirectToAction("Index", new { id = id });
            }


            return View(newMotorbike);
        }

        public IActionResult ServisManage()
        {
            return View();
        }

        public IActionResult ServisDetail(int servisID)
        {
            return View();
        } 

    }
}
