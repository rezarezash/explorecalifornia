using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreCalifornia.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Description = "First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Description = "Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Description = "User name")]
        public string UserName { get; set; }

    }
}
