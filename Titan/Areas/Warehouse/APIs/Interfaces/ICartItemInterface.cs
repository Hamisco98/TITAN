using Titan.Areas.Warehouse.Models;

namespace Titan.Areas.Warehouse.APIs.Interfaces
{
    public interface ICartItemInterface
    {
        Task<IEnumerable<CartItem>> GETALL();
        Task<CartItem> GETBYID(Guid id);
        Task<CartItem> POST(CartItem cartItem);
        Task<CartItem> PUT(CartItem cartItem);
        Task<CartItem> DELETE(Guid id);
    }
}
