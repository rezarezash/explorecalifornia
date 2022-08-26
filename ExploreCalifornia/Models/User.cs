using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreCalifornia.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(30)]
        [Display(Description ="First name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Description = "Last name")]
        public string LastName { get; set; }
        
        [Required]
        [NotMapped]
        [DataType(DataType.Password)]
        public string  Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [NotMapped]
        public string PasswordConfirm { get; set; }
       
    }
}
