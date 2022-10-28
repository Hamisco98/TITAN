using System.ComponentModel.DataAnnotations;

namespace Titan.Areas.Warehouse.Models
{
    public class ShoppingCart
    {
        [Key]
        public Guid ID { get; set; }
        public List<CartItem>? CartItem { get; set; }
        public double TotalCosts { get; set; } = 0;
        public double TotalPrices { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsSuccessful { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
