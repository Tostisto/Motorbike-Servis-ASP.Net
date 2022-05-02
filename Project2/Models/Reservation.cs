using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public int userId { get; set; }

        [Required]
        [Display(Name = "Motorbike")]
        public string? MotorbikeId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public System.DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public System.DateTime EndDate { get; set; }

        public string status { get; set; } = "new";
    }
}
