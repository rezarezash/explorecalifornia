using System.ComponentModel.DataAnnotations;

namespace ExploreCalifornia.ViewModels
{
    public class UserLoginViewModel
    {       
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        [Required]
        [MaxLength(100)]
        [Display(Description = "User name")]
        public string UserName { get; set; }
    }
}
