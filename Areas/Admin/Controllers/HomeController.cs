using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Models;
using T4RMSSolution.ViewModels;

namespace ReservationSystem.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        private readonly ApplicationDbContext _dbContext;
        public HomeController(ApplicationDbContext dbContext,RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) : base(roleManager, userManager)
        {
            this._dbContext = dbContext;
        }
        public IActionResult Report()
        {
            return View();
        }
        public List<ReportDataModel> ReservationsData()

        {
            var data = new List<ReportDataModel>();
            var reservations = _dbContext.Reservations
                .Where(r=>r.DateTime.Year == DateTime.Now.Year)
                .Select(a => new { Description = a.Sitting.SittingType.Description })
                .GroupBy(r => r.Description)
                .Select(group => new
                {
                    Key = group.Key,
                    Count = group.Count()
                })
                .ToList();

            foreach (var r in reservations)
            {

                var d = new ReportDataModel() { SittingType = r.Key, TotalReservation = r.Count };
                data.Add(d);

            }

            return data;

        }
    }
}