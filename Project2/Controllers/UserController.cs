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
            int userID = this.HttpContext.Session.GetInt32("adminID") ?? 0;

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
        public IActionResult Servis(int id, Servis servis)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
                {
                    using (SqliteCommand cmd = new SqliteCommand())
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


        public IActionResult Order()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Order> userOrders = new List<Order>();

            using(SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using(SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Orders WHERE UserID = @UserID and Status = 'new'";
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order();
                            order.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            order.userID = reader.GetInt32(reader.GetOrdinal("UserID"));
                            order.Product = reader.GetString(reader.GetOrdinal("Product"));
                            order.Service = reader.GetString(reader.GetOrdinal("Service"));
                            order.Price = reader.GetInt32(reader.GetOrdinal("Price"));
                            order.Status = reader.GetString(reader.GetOrdinal("Status"));

                            userOrders.Add(order);
                        }
                    }
                }
                conn.Close();
            }
            ViewBag.userOrders = userOrders;

            userOrders.Count();

            return View();
        }


        public IActionResult Dismiss(int id)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "Delete from Orders where Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return RedirectToAction("Order", "User");
        }
        

        public IActionResult PayAll()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }


            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Orders SET Status = 'paid' WHERE UserID = @UserID and Status = 'new'";
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return RedirectToAction("Order", "User");
        }



        public void AddMotorbikeOption()
        {
            List<Motorbike> motorbikes = new List<Motorbike>();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from Motorbike";

                    SqliteDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Motorbike motorbike = new Motorbike();
                        motorbike.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        motorbike.Name = reader.GetString(reader.GetOrdinal("Name"));
                        motorbike.Price = reader.GetInt32(reader.GetOrdinal("Price"));
                        motorbike.Description = reader.GetString(reader.GetOrdinal("Description"));
                        
                        if (reader.IsDBNull(reader.GetOrdinal("Image")))
                        {
                            motorbike.Image = null;
                        }
                        else
                        {
                            motorbike.Image = reader.GetString(reader.GetOrdinal("Image"));
                        }

                        motorbikes.Add(motorbike);
                    }
                }
                conn.Close();
            }

            ViewBag.motorbikes = motorbikes;

            List<SelectListItem> SelectMotorbikes = new List<SelectListItem>();

            foreach (Motorbike motorbike in motorbikes)
            {
                SelectMotorbikes.Add(new SelectListItem { Text = motorbike.Name, Value = motorbike.Id.ToString() });
            }

            ViewBag.SelectMotorbikes = SelectMotorbikes;
        }


        
[       HttpGet]
        public IActionResult Reservation()
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            AddMotorbikeOption();

            return View();
        }

        [HttpPost]
        public IActionResult Reservation(Reservation form)
        {
            int userID = this.HttpContext.Session.GetInt32("userID") ?? 0;

            if (userID == 0)
            {
                return RedirectToAction("Index", "Home");
            }


            AddMotorbikeOption();
            
            if(ModelState.IsValid)
            {
                using(SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
                {
                    using (SqliteCommand cmd = new SqliteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Reservations (UserID, MotorbikeID, From_Date, To_Date, Status) VALUES (@UserID, @MotorbikeID, @From, @To, @Status)";
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@MotorbikeID", form.MotorbikeId);
                        cmd.Parameters.AddWithValue("@From", form.StartDate);
                        cmd.Parameters.AddWithValue("@To", form.EndDate);
                        cmd.Parameters.AddWithValue("@Status", "new");

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    Motorbike selectedMotorbike = new Motorbike();  

                    using (SqliteCommand cmd = new SqliteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * FROM Motorbike WHERE Id = @Id";
                        cmd.Parameters.AddWithValue("@Id", form.MotorbikeId);

                        SqliteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            selectedMotorbike.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            selectedMotorbike.Name = reader.GetString(reader.GetOrdinal("Name"));
                            selectedMotorbike.Price = reader.GetInt32(reader.GetOrdinal("Price"));
                            selectedMotorbike.Description = reader.GetString(reader.GetOrdinal("Description"));

                            if (reader.IsDBNull(reader.GetOrdinal("Image")))
                            {
                                selectedMotorbike.Image = null;
                            }
                            else
                            {
                                selectedMotorbike.Image = reader.GetString(reader.GetOrdinal("Image"));
                            }
                        }
                    }


                    using (SqliteCommand cmd = new SqliteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Orders (UserId, Product, Service, Price,Status) VALUES (@UserId, @Product, @Service, @Price, @Status)";
                        cmd.Parameters.AddWithValue("@UserId", userID);
                        cmd.Parameters.AddWithValue("@Product", selectedMotorbike.Name);
                        cmd.Parameters.AddWithValue("@Service", "Reservation");
                        cmd.Parameters.AddWithValue("@Price", selectedMotorbike.Price * 5); // TODO: add days from input
                        cmd.Parameters.AddWithValue("@Status", "new");
                        
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                return RedirectToAction("Index", "User");
            }    
            
            return View();
        }

    }
}
