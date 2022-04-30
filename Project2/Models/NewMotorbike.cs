using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class NewMotorbike
    {
        [Required]
        [Display(Name = "Motorbike Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Motorbike Price")]
        public int Price { get; set; }
        [Required]
        [Display(Name = "Motorbike Description")]
        public string Description { get; set; }
        [Display(Name = "Motorbike Image")]
        public string? Image { get; set; }

    }
}
