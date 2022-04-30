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
        public IActionResult Index(LoginForm form)
        {
            if (ModelState.IsValid)
            {
                int UserID = 0;
                string UserRole = "";
                
                using (SqliteConnection conn = new SqliteConnection("Filename=Project2.db"))
                {
                    conn.Open();

                    using(SqliteCommand cmd = new SqliteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select * from User where Email = $email and Password = $pass";
                        cmd.Parameters.AddWithValue("$email", form.Email);
                        cmd.Parameters.AddWithValue("$pass", form.Password);

                        using (SqliteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("Id"));
                                UserRole = reader.GetString(reader.GetOrdinal("Role"));
                            }
                        }
                    }
                    conn.Close();
                }

                if (UserID != 0)
                {
                    if (UserRole == "Admin")
                    {
                        return RedirectToAction("Index", "Admin", new { id = UserID });
                    }
                    else if (UserRole == "User")
                    {
                        return RedirectToAction("Index", "User", new { id = UserID });
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
        public IActionResult Register(RegisterForm form)
        {
            if (ModelState.IsValid)
            {
                using(SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
                {
                    conn.Open();

                    using (SqliteCommand cmd = new SqliteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO User (FirstName, LastName, Email, Password,Role) VALUES (@FirstName, @LastName, @Email, @Password, @Role)";
                        cmd.Parameters.AddWithValue("@FirstName", form.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", form.LastName);
                        cmd.Parameters.AddWithValue("@Email", form.Email);
                        cmd.Parameters.AddWithValue("@Password", form.Password);
                        cmd.Parameters.AddWithValue("@Role", "User");

                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return RedirectToAction("Index", "Home");
            }
            return View(form);
        }

    }
}
