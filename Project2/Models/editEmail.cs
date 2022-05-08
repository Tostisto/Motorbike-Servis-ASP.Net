using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class editEmail
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
