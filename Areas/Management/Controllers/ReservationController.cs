using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Models;
using ReservationSystem.Repos;
using ReservationSystem.ViewModels;

namespace ReservationSystem.Areas.Management.Controllers
{
    [Area("Management")]
    public class ReservationController : ReservationRepos
    {
        public ReservationController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) : base(dbContext, userManager)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ts = await RType();
            var r = new ReservationCreateViewModel
            {
                ReservationTypes = ts.ToList(),
            };
            return View(r);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel r)
        {
            var t = await RType();
            r.ReservationTypes = t.ToList();
            var c = await CustomerValidation(r.FirstName,r.LastName, r.PhoneNumber,null);
            
            if (true)
            {
                var reservation = new Reservation
                {
                    //SittingId = sitting.SittingId,
                    ReservationTypeId = r.ReservationTypeId,                    
                    Customer = c,
                    CustomerId = c.Id,
                    Guests = r.Guests,
                    DateTime = r.DateTime,
                };
                if (ModelState.IsValid)
                {
                    await AddReservation(reservation);
                    return RedirectToAction("ReservationIndex", new {id = reservation.SittingId});
                }
                else
                {
                    ViewBag.EnterError = "Make sure everything is right before click submit";
                    return View(r);
                }
            }
            ViewBag.ErrorMessage = "This time or day is not avaiable try other time";
            return View(r);
        }
        public IActionResult Index()
        {

            return View();
        }

    }
}