using Microsoft.Data.Sqlite;
using Project2.Models;

namespace Project2
{
    public class DatabaseOperations
    {
        public static async Task NewMotorbike(Motorbike newMotorbike)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Motorbike (Name, Price, Description, Image) VALUES (@Name, @Price, @Description, @Image)";

                    cmd.Parameters.AddWithValue("@Name", newMotorbike.Name);
                    cmd.Parameters.AddWithValue("@Price", newMotorbike.Price);
                    cmd.Parameters.AddWithValue("@Description", newMotorbike.Description);
                    if (newMotorbike.Image == "" || newMotorbike.Image == null)
                    {
                        cmd.Parameters.AddWithValue("@Image", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Image", newMotorbike.Image);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }
                conn.Close();
            }
        }


        public static async Task<List<Servis>> AllServis()
        {
            List<Servis> servises = new List<Servis>();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Services where status = 'new'";

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

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
            return servises;
        }


        public static async Task<Tuple<Servis, User>> ServisDetail(int id)
        {
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

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

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

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        user.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        user.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                        user.Email = reader.GetString(reader.GetOrdinal("Email"));

                    }
                }
                conn.Close();
            }
            return Tuple.Create(servis, user);
        }


        public static async Task ServisStatus(int id, string status)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Services SET Status = @status WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@status", status);

                    await cmd.ExecuteNonQueryAsync();
                }
                conn.Close();
            }
        }

        public static async Task<List<Motorbike>> AllMotorbikes()
        {
            List<Motorbike> motorbikes = new List<Motorbike>();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    //cmd.CommandText = "SELECT * FROM Motorbike";

                    cmd.CommandText = @"select count(Reservations.Id), Motorbike.Id, Motorbike.Name, Motorbike.Price, Motorbike.Description, Motorbike.Image from Motorbike left join Reservations on Motorbike.Id = Reservations.MotorbikeID group by Motorbike.Name";

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

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

                        int count = reader.GetInt32(reader.GetOrdinal("count(Reservations.Id)"));

                        if (count == 0)
                        {
                            motorbike.isReserved = false;
                        }
                        else
                        {
                            motorbike.isReserved = true;
                        }

                        motorbikes.Add(motorbike);
                    }
                }
                conn.Close();
            }
            return motorbikes;
        }


        public static async Task<bool> MotorbikeReservationCheck(int id)
        {
            bool check = false;

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT count(*) FROM Reservations where MotorbikeId = @Id and To_Date > @date";

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);

                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    if (count > 0)
                    {
                        check = true;
                    }
                }
            }
            return check;
        }


        public static async Task RemoveMotorbike(int id)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM Reservations where MotorbikeID = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    await cmd.ExecuteNonQueryAsync();
                }

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM Motorbike WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    await cmd.ExecuteNonQueryAsync();
                }

                conn.Close();
            }
        }

        public static async Task<Motorbike> SelectMotorbike(int id)
        {
            Motorbike motorbike = new Motorbike();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Motorbike WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        motorbike.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        motorbike.Name = reader.GetString(reader.GetOrdinal("Name"));
                        motorbike.Price = reader.GetInt32(reader.GetOrdinal("Price"));
                        motorbike.Description = reader.GetString(reader.GetOrdinal("Description"));
                        motorbike.Image = reader.GetString(reader.GetOrdinal("Image"));
                    }
                    conn.Close();
                }

                return motorbike;
            }
        }

        public static async Task EditMotorbike(Motorbike motorbike)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Motorbike SET Name = @Name, Price = @Price, Description = @Description, Image = @Image WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", motorbike.Id);
                    cmd.Parameters.AddWithValue("@Name", motorbike.Name);
                    cmd.Parameters.AddWithValue("@Price", motorbike.Price);
                    cmd.Parameters.AddWithValue("@Description", motorbike.Description);
                    cmd.Parameters.AddWithValue("@Image", motorbike.Image);

                    await cmd.ExecuteNonQueryAsync();
                }
                conn.Close();
            }
        }


        // User
        public static async Task NewServis(Servis servis, int userID)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Services (UserID, Brand, Model, Year, SPZ, Shop, Description, Status) VALUES (@UserID, @Brand, @Model, @Year, @SPZ, @Shop, @Description, @Status)";
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Brand", servis.Brand);
                    cmd.Parameters.AddWithValue("@Model", servis.Model);
                    cmd.Parameters.AddWithValue("@Year", servis.Year);
                    cmd.Parameters.AddWithValue("@SPZ", servis.SPZ);
                    cmd.Parameters.AddWithValue("@Shop", servis.Shop);
                    cmd.Parameters.AddWithValue("@Description", servis.Description);
                    cmd.Parameters.AddWithValue("@Status", "new");

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                }
            }
        }


        public static async Task<List<Order>> UserOrder(int userID)
        {
            List<Order> userOrders = new List<Order>();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Orders WHERE UserID = @UserID and Status = 'new'";
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    using (SqliteDataReader reader = await cmd.ExecuteReaderAsync())
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
            return userOrders;
        }

        public static async Task RemoveOrder(int id)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "Delete from Orders where Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                }
            }
        }


        public static async Task PayAllOrders(int userID)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Orders SET Status = 'paid' WHERE UserID = @UserID and Status = 'new'";
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                }
            }
        }

        public static async Task<List<Motorbike>> AllMotorbike()
        {
            List<Motorbike> motorbikes = new List<Motorbike>();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from Motorbike";

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

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
            return motorbikes;
        }


        public static async Task NewReservation(Reservation reservation, int userID)
        {
            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Reservations (UserID, MotorbikeID, From_Date, To_Date, Status) VALUES (@UserID, @MotorbikeID, @From, @To, @Status)";
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@MotorbikeID", reservation.MotorbikeId);
                    cmd.Parameters.AddWithValue("@From", reservation.StartDate);
                    cmd.Parameters.AddWithValue("@To", reservation.EndDate);
                    cmd.Parameters.AddWithValue("@Status", "new");

                    await cmd.ExecuteNonQueryAsync();
                }

                Motorbike selectedMotorbike = new Motorbike();

                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Motorbike WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", reservation.MotorbikeId);

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

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

                    await cmd.ExecuteNonQueryAsync();
                }
                conn.Close();
            }
        }


        // Office
        public static async Task<List<Office>> AllOffices()
        {
            List<Office> offices = new List<Office>();

            using (SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from BranchOffice";

                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        Office office = new Office();
                        office.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        office.City = reader.GetString(reader.GetOrdinal("City"));
                        office.Address = reader.GetString(reader.GetOrdinal("Address"));

                        offices.Add(office);
                    }
                }
                conn.Close();
            }
            return offices;
        }

        public static async Task<Office> GetSpecificOffice(int id)
        {
            Office office = new Office();

            using(SqliteConnection conn = new SqliteConnection("Data Source=Project2.db"))
            {
                using(SqliteCommand cmd = new SqliteCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM BranchOffice WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    conn.Open();
                    SqliteDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        office.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        office.City = reader.GetString(reader.GetOrdinal("City"));
                        office.Address = reader.GetString(reader.GetOrdinal("Address"));
                    }
                    conn.Close();
                }
            }
            return office;
        }
        
    }
}