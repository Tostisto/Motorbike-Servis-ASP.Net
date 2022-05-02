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


        [HttpGet]
        public IActionResult NewMotorbike()
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult NewMotorbike(Motorbike newMotorbike)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

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

                return RedirectToAction("Index");
            }
            return View(newMotorbike);
        }

        
        [HttpGet]
        public IActionResult ServisManage()
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Servis> servises = new List<Servis>(); 

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Services where status = 'new'";

                    SqliteDataReader reader = cmd.ExecuteReader();

                    ViewBag.servis = new List<Servis>();

                    while (reader.Read())
                    {
                        Servis servis = new Servis();
                        servis.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        servis.userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        servis.Brand = reader.GetString(reader.GetOrdinal("Brand"));
                        servis.Model = reader.GetString(reader.GetOrdinal("Model"));
                        servis.Year = reader.GetInt32(reader.GetOrdinal("Year"));
                        servis.SPZ = reader.GetString(reader.GetOrdinal("SPZ"));
                        servis.Shop = reader.GetString(reader.GetOrdinal("Shop"));
                        servis.Status = reader.GetString(reader.GetOrdinal("Status"));

                        servises.Add(servis);
                    }
                }
                conn.Close();
            }
            ViewBag.servises = servises;

            return View();
        }

        public IActionResult Detail(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            
            Servis servis = new Servis();
            User user = new User();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Services where Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    SqliteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        servis.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        servis.userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                        servis.Brand = reader.GetString(reader.GetOrdinal("Brand"));
                        servis.Model = reader.GetString(reader.GetOrdinal("Model"));
                        servis.Year = reader.GetInt32(reader.GetOrdinal("Year"));
                        servis.SPZ = reader.GetString(reader.GetOrdinal("SPZ"));
                        servis.Shop = reader.GetString(reader.GetOrdinal("Shop"));
                        servis.Status = reader.GetString(reader.GetOrdinal("Status"));
                    }                    
                }

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from User where Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", servis.userId);

                    SqliteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        user.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        user.Email = reader.GetString(reader.GetOrdinal("Email"));

                    }
                }
                conn.Close();
            }

            ViewBag.servis = servis;
            ViewBag.user = user;

            return View();
        } 

        public IActionResult Accept(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Services SET Status = 'accepted' WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

            return RedirectToAction("ServisManage");
        }

        public IActionResult Dismiss(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Services SET Status = 'dismissed' WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
            return RedirectToAction("ServisManage");
        }
    }
}
