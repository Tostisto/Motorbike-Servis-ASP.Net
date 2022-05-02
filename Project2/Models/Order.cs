namespace Project2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int userID { get; set; }
        public string Product { get; set; }
        public string Service { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
    }
}
