using System;
using System.ComponentModel.DataAnnotations;

namespace ExploreCalifornia.Models
{
    public class Post
    {
        [Key]
        public long Id { get; set; }

        [Display(Name ="Post Tilte")]
        [MaxLength(100)]
        [Required]        
        public string Title { get; set; }
        
        [Required]
        [Display(Name = "Author's Name")]
        public string Author { get; set; }

        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostedOn { get; set; } = DateTime.Now;
    }
}
