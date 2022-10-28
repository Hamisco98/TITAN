using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Titan.Areas.Warehouse.APIs.Interfaces;

namespace Titan.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Roles = "Owner,Admin,DataEntry,Cashier")]
    public class CartsController : Controller
    {
        private readonly IShoppingCartInterface shoppingCartInterface;
        private readonly ICartItemInterface cartItemInterface;

        public CartsController(IShoppingCartInterface shoppingCartInterface, ICartItemInterface cartItemInterface)
        {
            this.shoppingCartInterface = shoppingCartInterface;
            this.cartItemInterface = cartItemInterface;
        }
        [Authorize(Roles = "Owner,Admin,Cashier")]
        public async Task<IActionResult> ShoppingCarts()
        {
            return View();
        }

        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Sales()
        {
            return View();
        }
    }
}
