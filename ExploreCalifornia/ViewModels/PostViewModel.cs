using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ExploreCalifornia.ViewModels
{
    public class PostViewModel
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "Post Tilte")]
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Author's Name")]
        public string AuthorName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
