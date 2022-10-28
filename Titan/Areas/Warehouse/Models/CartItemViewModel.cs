namespace Titan.Areas.Warehouse.Models
{
    public class CartItemViewModel
    {
        public string barcode { get; set; }
        public Guid shoppingCartID { get; set; }
        public int quantity { get; set; } = 1;
        public Guid productID { get; set; }
    }
}
