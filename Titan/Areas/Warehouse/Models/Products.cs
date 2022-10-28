namespace Titan.Areas.Warehouse.Models
{
    public class Products
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
