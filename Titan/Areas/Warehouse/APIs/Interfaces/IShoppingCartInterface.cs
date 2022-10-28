using Titan.Areas.Warehouse.Models;

namespace Titan.Areas.Warehouse.APIs.Interfaces
{
    public interface IShoppingCartInterface
    {
        Task<IEnumerable<ShoppingCart>> GETALL();
        Task<ShoppingCart> GETBYID(Guid id);
        Task<ShoppingCart> POST(ShoppingCart shoppingCart);
        Task<ShoppingCart> PUT(ShoppingCart shoppingCart);
        Task<ShoppingCart> DELETE(Guid id);
    }
}
