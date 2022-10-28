using Microsoft.EntityFrameworkCore;
using Titan.Areas.Warehouse.APIs.Interfaces;
using Titan.Areas.Warehouse.Models;
using Titan.Data;

namespace Titan.Areas.Warehouse.APIs.Repositories
{
    public class CartItemRepository : ICartItemInterface
    {
        private readonly ApplicationDbContext context;

        public CartItemRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<CartItem>> GETALL()
        {
            return await context.CartItem.ToListAsync();
        }

        public async Task<CartItem> GETBYID(Guid id)
        {
            return await context.CartItem.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<CartItem> POST(CartItem cartItem)
        {
            var _cartItem = await context.CartItem.AddAsync(cartItem);
            await context.SaveChangesAsync();
            return _cartItem.Entity;
        }

        public async Task<CartItem> PUT(CartItem cartItem)
        {
            var _cartItem = await context.CartItem.FirstOrDefaultAsync(p => p.ID == cartItem.ID);
            if (_cartItem != null)
            {
                cartItem.ProductID = cartItem.ProductID;
                _cartItem.ProductBarcode = cartItem.ProductBarcode;
                _cartItem.ProductName = cartItem.ProductName;
                _cartItem.Cost = cartItem.Cost;
                _cartItem.Price = cartItem.Price;
                _cartItem.Quantity = cartItem.Quantity;
                _cartItem.SubTotalCost = cartItem.SubTotalCost;
                _cartItem.SubTotalPrice = cartItem.SubTotalPrice;
                _cartItem.Date = DateTime.Now;
                _cartItem.ShoppingCartID = cartItem.ShoppingCartID;

                await context.SaveChangesAsync();
                return _cartItem;
            }
            else await POST(cartItem);
            return null;
        }
         public async Task<CartItem> DELETE(Guid id)
        {
            var _cartItem = await context.CartItem.FirstOrDefaultAsync(p => p.ID == id);
            if (_cartItem != null)
            {
                context.CartItem.Remove(_cartItem);
                await context.SaveChangesAsync();
                return _cartItem;
            }
            return null;
        }
    }
}
