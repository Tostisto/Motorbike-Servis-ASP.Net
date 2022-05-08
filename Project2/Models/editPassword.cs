using System.ComponentModel.DataAnnotations;

namespace Project2.Models
{
    public class editPassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string currentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string newPassword { get; set; }

    }
}
