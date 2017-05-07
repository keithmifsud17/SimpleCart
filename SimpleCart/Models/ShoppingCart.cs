using System.ComponentModel.DataAnnotations;

namespace SimpleCart.Models
{
    public class ShoppingCart
    {
        public int ID { get; set; }

        [Required]
        public int UserId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
