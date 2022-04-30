using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class Servis
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string SPZ { get; set; }
        public string Shop { get; set; }
        public string Description { get; set; }
    }
}
