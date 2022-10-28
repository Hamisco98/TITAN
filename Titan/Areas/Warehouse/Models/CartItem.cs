using System.ComponentModel.DataAnnotations;

namespace Titan.Areas.Warehouse.Models
{
    public class CartItem
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        public string ProductBarcode { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public double Cost { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; } = 1;
        public double SubTotalCost { get; set; }
        public double SubTotalPrice { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Guid ShoppingCartID { get; set; }
    }
}
