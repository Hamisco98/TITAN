using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Titan.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    //[Authorize(Roles = "Owner,Admin,DataEntry")]
    [Authorize(Roles = "Admin,Owner,DataEntry,Investors")]
    public class WarehouseController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
