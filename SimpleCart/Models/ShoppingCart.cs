namespace SimpleCart.Models
{
    public class ShoppingCart
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
