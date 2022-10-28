using Microsoft.EntityFrameworkCore;
using Titan.Areas.Warehouse.APIs.Interfaces;
using Titan.Areas.Warehouse.Models;
using Titan.Data;

namespace Titan.Areas.Warehouse.APIs.Repositories
{
    public class ShoppingCartRepositories : IShoppingCartInterface
    {
        private readonly ApplicationDbContext context;

        public ShoppingCartRepositories(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<ShoppingCart>> GETALL()
        {
            return await context.ShoppingCarts.ToListAsync();
        }

        public async Task<ShoppingCart> GETBYID(Guid id)
        {
            return await context.ShoppingCarts.FirstOrDefaultAsync(e => e.ID == id);
        }
        
        public async Task<ShoppingCart> POST(ShoppingCart cartItem)
        {
            var _cartItem = await context.ShoppingCarts.AddAsync(cartItem);
            await context.SaveChangesAsync();
            return _cartItem.Entity;
        }

        public async Task<ShoppingCart> PUT(ShoppingCart cartItem)
        {
            var _cartItem = await context.ShoppingCarts.FirstOrDefaultAsync(p => p.ID == cartItem.ID);
            if (_cartItem != null)
            {
                _cartItem.ID = cartItem.ID;
                _cartItem.TotalPrices = cartItem.TotalPrices;
                _cartItem.TotalCosts = cartItem.TotalCosts;
                _cartItem.Date = DateTime.Now;
                _cartItem.CartItem = cartItem.CartItem;
                _cartItem.IsSuccessful = cartItem.IsSuccessful;
                _cartItem.IsActive = cartItem.IsActive;
                await context.SaveChangesAsync();
                return _cartItem;
            }
            return null;
        }

        public async Task<ShoppingCart> DELETE(Guid id)
        {
            var _cartItem = await context.ShoppingCarts.FirstOrDefaultAsync(p => p.ID == id);
            if (_cartItem != null)
            {
                context.ShoppingCarts.Remove(_cartItem);
                await context.SaveChangesAsync();
                return _cartItem;
            }
            return null;
        }
    }
}
