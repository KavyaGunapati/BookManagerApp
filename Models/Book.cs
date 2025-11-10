using System.ComponentModel.DataAnnotations;

namespace BookManagerApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        [Range(0,10000)]
        public Decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
       
    }
}
