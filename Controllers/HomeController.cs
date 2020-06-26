using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReservationSystem.Models;
using ReservationSystem.Repos;
using ReservationSystem.ViewModels;

namespace ReservationSystem.Controllers
{
    public class HomeController : ReservationRepos
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) : base(dbContext, userManager)
        {
            _logger = logger;
        }



        [HttpGet]
        public IActionResult Booking()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(ReservationCreatViewModelClient r)
        {
            if (!ModelState.IsValid)
            {
                return View(r);
            }
            var t = await RType();
            var rs = await Status();
            var id = t.FirstOrDefault(n => n.Description.ToLower().Contains("online")).Id;
            var rsid = rs.FirstOrDefault(n => n.Description.ToLower().Contains("pending")).Id;
            var c = await CustomerValidation(r.FirstName,r.LastName, r.PhoneNumber, r.Email);
            

            if ( await IsDateValid(r.DateTime))
            {
                var s = await SittingByDate(r.DateTime);
                var ts = await TablesCollection(r.DateTime);
                var rom = new Random();
                var reservation = new Reservation
                {
                    SittingId = s.SittingId,
                    ReservationTypeId = id,
                    ReservationStatusId = rsid,
                    TableId =rom.Next(ts[s.SittingId].Min(), ts[s.SittingId].Length),
                    Customer = c,
                    CustomerId = c.Id,
                    Guests = r.Guests,
                    DateTime = r.DateTime,
                    Notes = r.Notes
                };
                if (ModelState.IsValid)
                {
                    await AddReservation(reservation);
                    return RedirectToAction("Thankyou");
                }

                return View(r);
            }

            ViewBag.ErrorMessage = "This time or day you chose is not available, please try other time.";
            return View(r);

        }


        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Thankyou()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult RedirectUser()
        {

            if (User.IsInRole("Staff"))
            {
                return RedirectToAction("Index", "Home", new { area = "Management" });
            }
            else if (User.IsInRole("Member"))
            {
                return RedirectToAction("Index", "Home", new { area = "Member" });
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return LocalRedirect("/");
        }
    }

}
