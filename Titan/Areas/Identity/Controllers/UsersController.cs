using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Titan.Areas.Identity.ViewModels;
using Titan.Models.InheritModels;

namespace Titan.Areas.Identity.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    [Area("Identity")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> GetUsers()
        {
            var user = await userManager.Users.Select(user => new UserViewModel
            {
                ID = user.Id,
                Name = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Username = user.UserName,
                ImagePicture = user.ProfilePicture,
                Roles = userManager.GetRolesAsync(user).Result
            }).ToListAsync();

            return View(user);
        }
        
        public async Task<IActionResult> ManageRoles(string userId)
        {
            if(userId == null) return NotFound();
            
            var user = await userManager.FindByIdAsync(userId);
            
            if(user == null) return NotFound();

            var roles = await roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserID = user.Id,
                Username = user.UserName,
                Roles = roles.Select(r => new RoleViewModel
                {
                    RoleID = r.Id,
                    RoleName = r.Name,
                    IsSelected = userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()};
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserID);
            if(user == null) return NotFound();

            if(!ModelState.IsValid) return View(model);

            for (int i = 0; i < model.Roles.Count; i++)
            {
                if (model.Roles[i].IsSelected && !userManager.IsInRoleAsync(user, model.Roles[i].RoleName).Result) await userManager.AddToRoleAsync(user, model.Roles[i].RoleName);
                if (!model.Roles[i].IsSelected && userManager.IsInRoleAsync(user, model.Roles[i].RoleName).Result) await userManager.RemoveFromRoleAsync(user, model.Roles[i].RoleName);
            }

            //var userRoles = await userManager.GetRolesAsync(user);
            //foreach (var role in model.Roles)
            //{
            //    //if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected) await userManager.RemoveFromRolesAsync(user);
            //}

            return RedirectToAction(nameof(GetUsers));
        }
    }
}
