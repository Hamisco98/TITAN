using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Titan.Areas.Identity.ViewModels;

namespace Titan.Areas.Identity
{
    [Authorize(Roles = "Admin,Owner")]
    [Area("Identity")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> GetRoles() => View(await roleManager.Roles.ToListAsync());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RoleFormViewModel role)
        {
            if(!ModelState.IsValid) return View(nameof(GetRoles), await roleManager.Roles.ToListAsync());
            if(await roleManager.RoleExistsAsync(role.RoleName))
            {
                ModelState.AddModelError("RoleName", "This Role Is Already Exists!");
                return View(nameof(GetRoles), await roleManager.Roles.ToListAsync());
            }
            //Make Toster In Case Model.IsValid and If Exist and If Added
            await roleManager.CreateAsync(new IdentityRole(role.RoleName.Trim()));
            return RedirectToAction(nameof(GetRoles));
        }
    }
}
