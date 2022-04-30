using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class RegisterForm
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
