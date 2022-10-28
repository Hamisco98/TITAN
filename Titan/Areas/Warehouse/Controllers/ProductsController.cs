using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Titan.Areas.Warehouse.Data.Interfaces;
using Titan.Areas.Warehouse.Models;

namespace Titan.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    //[Authorize(Roles = "Owner,Admin,DataEntry")]
    [Authorize(Roles = "Admin,Owner,DataEntry,Cashier")]
    public class ProductsController : Controller
    {
        private readonly IProductInterface productInterface;

        public ProductsController(IProductInterface productInterface)
        {
            this.productInterface = productInterface;
        }
        public async Task<IActionResult> Index()
        {
            var prd = await productInterface.GETALL();
            prd = prd.OrderByDescending(o => o.Date).ToList();
            return View(prd);
        }
    }
}
