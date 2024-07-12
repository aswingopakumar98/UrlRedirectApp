using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlRedirectApp.Data.Models
{
    public class UrlMapping
    {
        [Key]
        [Required]
        public int id
        {
            get; set;
        }

        [Required]
        [StringLength(255)]
        public string LongUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string shortUrl { get; set; }

        [Required]
        public int Clicks { get; set; }




    }
}