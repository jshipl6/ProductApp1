using System.ComponentModel.DataAnnotations;

namespace ProductApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal Price { get; set; }
    }
}
