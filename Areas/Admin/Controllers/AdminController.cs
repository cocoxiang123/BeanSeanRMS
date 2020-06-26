using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Areas.Admin.Models;



namespace ReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

       

        public ViewResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole r)
        {
            await roleManager.CreateAsync(r);
            return RedirectToAction("RoleIndex");
            
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DeleteRoleIndex(string id)
        {
            var r = await roleManager.FindByIdAsync(id);
            return View(r);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRoleIndex(IdentityRole r)
        {
             var role = await roleManager.FindByNameAsync(r.Name);
                await roleManager.DeleteAsync(role);

             return RedirectToAction("RoleIndex");
        }
        public IActionResult RoleIndex()
        {
            var r =  roleManager.Roles;
            return View(r);
        }
        public async Task<IActionResult> UserRoleIndex(string id)
        {
            var r = await roleManager.FindByIdAsync(id);
            ViewBag.RoleName = r.Name;
            ViewBag.Id = id;
            var users = await GetUserWithRoleAssigned(id);
            var u = users.Where(u => u.flag == true).ToList();
            return View(u);
        }
        public async Task<IActionResult> AssignUser(string id)
        {
            ViewBag.Id = id;
            var users = await GetUserWithRoleAssigned(id);
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult>AssignUser(string id, List<UserViewModel> users)
        {
            var r = await roleManager.FindByIdAsync(id);

            for (int i = 0; i < users.Count; i++)
            {               
                
                var u = await userManager.FindByIdAsync(users[i].Id);
                if (users[i].flag&&!(await userManager.IsInRoleAsync(u, r.Name)))
                {
                     await userManager.AddToRoleAsync(u, r.Name);
                }
                else if (!users[i].flag&&(await userManager.IsInRoleAsync(u, r.Name)))
                {
                     await userManager.RemoveFromRoleAsync(u, r.Name);
                }
                else
                {
                    continue;
                }
            }
            return RedirectToAction("UserRoleIndex",new { id = id}); 
        }
        private async Task<List<UserViewModel>> GetUserWithRoleAssigned(string id)
        {
            var r = await roleManager.FindByIdAsync(id);
            var newUsers = new List<UserViewModel>();
            foreach (var user in userManager.Users)
            {
                var vm = new UserViewModel
                {
                    Id = user.Id,
                    Name = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, r.Name))
                {
                    vm.flag = true;
                }
                else
                {
                    vm.flag = false;
                }
                newUsers.Add(vm);
            }
            return newUsers;
        }
    }
}