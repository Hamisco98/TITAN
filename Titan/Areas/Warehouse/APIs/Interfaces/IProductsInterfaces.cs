using Titan.Areas.Warehouse.Models;

namespace Titan.Areas.Warehouse.APIs.Interfaces
{
    public interface IProductsInterfaces
    {
        Task<IEnumerable<Products>> GETALL();
        Task<Products> GETBYID(Guid id);
        Task<Products> GETBARCODE(string id);
        Task<Products> POST(Products products);
        Task<Products> PUT(Products products);
        Task<Products> DELETE(Guid id);
        Task<IEnumerable<Products>> SEARCH(string name, string barcode, double price, double netPrice);
    }
}
