using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class Servis
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public int userId { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public string Brand { get; set; }
        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "SPZ")]
        public string SPZ { get; set; }
        [Required]
        [Display(Name = "Our Shop")]
        public string Shop { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; } = "new";
    }
}
