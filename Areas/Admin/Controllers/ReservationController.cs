using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationSystem.Models;
using ReservationSystem.Repos;
using ReservationSystem.ViewModels;
using T4RMSSolution.Models;
using T4RMSSolution.ViewModels;

namespace ReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ReservationController : ReservationRepos
    {

        public ReservationController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) : base(dbContext, userManager)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {

            var rts = await RType();
            var s = await Sitting(id);
            var ts = await TablesCollection(s.Start);
            var dts = await DynamicTableOutput(ts);
            var r = new ReservationCreateViewModel
            {
                SittingId = id,
                ReservationTypes = rts.ToList(),
                Tables = dts[s],
                DateTime = s.Start
            };
            return View(r);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationCreateViewModel r)
        {
            var s = await Sitting(r.SittingId);
            var tAarry = await TablesCollection(r.DateTime);
            var ts = await DynamicTableOutput(tAarry);
            var t = await RType();
            r.Tables = ts[s];
            var rs = await Status();
            var rsId = rs.FirstOrDefault(r => r.Description.ToLower().Contains("pending")).Id;
            r.ReservationTypes = t.ToList();
            var c = await CustomerValidation(r.FirstName, r.LastName, r.PhoneNumber, r.Email);
            if (s.Status)
                {
                    var reservation = new Reservation
                    {
                        SittingId =r.SittingId,
                        ReservationTypeId = r.ReservationTypeId,
                        ReservationStatusId = rsId,
                        TableId = r.TableId,
                        Customer = c,
                        CustomerId = c.Id,
                        Guests = r.Guests,
                        DateTime = r.DateTime,
                        Notes = r.Notes
                    };
                    if (ModelState.IsValid)
                    {
                        await AddReservation(reservation);
                        return RedirectToAction("ReservationIndex");
                    }

                    ViewBag.EnterError = "Make sure everything is right before click submit";
                    return View(r);
                }
                ViewBag.ErrorMessage = "This time or day is Fully booked try other time";

                return View(r);
            
        }

        public async Task<IActionResult> ControlPanel()
        {
            var s = await SittingCollection();
            ViewBag.SelectSittings = await SittingModels();
            return View(s);
        }
        [HttpPost]
        public async Task<IActionResult> ControlPanel(int id)
        {
            return RedirectToAction("ControlPanelMain", new { id = id });
        }
        public async Task<IActionResult> ControlPanelMain(int id)
        {
            var s = await Sitting(id);
            var ts = await TablesCollection(s.Start);
            await PressTheButton(ts);
            var f = await DynamicTableOutput(ts);
            return View(f);
        }
        /// <summary>
        /// FloorControl
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ControlPanelFLoor()
        {
            var s = await SittingCollection();
            return View(s);
        }
        [HttpPost]
        public async Task<IActionResult> ControlPanelFLoor(int id)
        {
            return RedirectToAction("ControlPanelFloorMain", new { id = id });
        }
        
        public async Task<IActionResult> ControlPanelFloorMain(int id)
        {
            return View(new ReservationControlViewModel{Reservations = await GetReservations(id)});
        }

        public ActionResult ManageTable()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageTable(TableCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            await AddingTables(vm.Quantity, vm.Location, vm.TableCapacity);
            return RedirectToAction("TableIndex");
        }
        public ActionResult ManageReservationStatus()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageReservationStatus(ReservationStatus r)
        {
            if (!ModelState.IsValid)
                return View(r);
            await AddReservationStatus(r);
            return RedirectToAction("ReservationIndex");
        }

        public async Task<IActionResult> TableIndex()
        {
            return View(await Tables());
        }
        public IActionResult Index()
        {

            return View();
        }


        public async Task<IActionResult> ReservationIndex()
        {

            return View(await GetReservations());
        }

        [HttpGet]
        public async Task<IActionResult> UpdateReservation(int id)
        {
            var r = await GetReservation(id);
            var model = new ReservationEditViewModel
            {
                SittingId = r.SittingId,
                CustomerId = r.CustomerId,
                ReservationTypeId = r.ReservationTypeId,
                Customer = r.Customer,
                DateTime = r.DateTime,
                Guests = r.Guests,
                Notes = r.Notes,
                FirstName = r.Customer.FirstName,
                LastName = r.Customer.LastName,
                Email = r.Customer.Email,
                PhoneNumber = r.Customer.PhoneNumber
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReservation(ReservationEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (await IsDateValid(vm.DateTime))
                {
                    var sitting = await SittingByDate(vm.DateTime);
                    var c = new Customer
                    {
                        Id = vm.CustomerId,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Email = vm.Email,
                        PhoneNumber = vm.PhoneNumber
                    };
                    await EditCustomer(c);
                    var r = new Reservation
                    {
                        SittingId = sitting.SittingId,
                        CustomerId = c.Id,
                        ReservationTypeId = vm.ReservationTypeId,
                        DateTime = vm.DateTime,
                        Guests = vm.Guests,
                        Notes = vm.Notes,
                    };
                    await EditReservation(r);
                    return RedirectToAction("ReservationIndex");
                }
                ViewBag.ErrorMessage = "This time or day is not avaiable try other time";
                return View(vm);

            }

            return View(vm);
        }
        public async Task<IActionResult> HandleConfirm(int id)
        {
            await Confirm(id);
            return RedirectToAction("HandlePending");
        }
        public async Task<IActionResult> HandleSeated(int id)
        {
            var Id =await Seated(id);
           
            return RedirectToAction("ControlPanelFloorMain", new {id=Id});
        }
        public async Task<IActionResult> HandleComplete(int id)
        {
            var Id = await Complete(id);
            return RedirectToAction("ControlPanelFloorMain", new { id = Id });
        }
        public async Task<IActionResult> HandleCancel(int id)
        {
            await Cancel(id);
            return RedirectToAction("HandlePending");
        }
        public async Task<IActionResult> HandleCancelFloor(int id)
        {
            await Cancel(id);
            return RedirectToAction("ControlPanelFloorMain", new { id = id });
        }
        public async Task<IActionResult> HandlePending()
        {
            var rs = await GetReservations();
            var result = rs.Where(r => r.DateTime <= DateTime.Now.AddDays(7) && r.ReservationStatusId == 1).ToList();
          
          return View(result);
        }

        

    }
}