using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class Motorbike
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Motorbike Name")]
        [Display(Name = "Motorbike Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the price")]
        [Display(Name = "Motorbike Price")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Please enter motorbike Description")]
        [Display(Name = "Motorbike Description")]
        public string Description { get; set; }
        
        [Display(Name = "Motorbike Image")]
        public string? Image { get; set; }
        
        public bool? isReserved { get; set; }
    }
}
