using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ReservationSystem.Models;
using ReservationSystem.ViewModels;
using Newtonsoft.Json;

namespace ReservationSystem.Controllers
{
    public class SittingController : Controller
    {
        private readonly ISittingRepository _sittingRepository;
        private readonly ApplicationDbContext context;

        public SittingController(ISittingRepository sittingRepository, ApplicationDbContext context)
        {
            _sittingRepository = sittingRepository;
            this.context = context;

        }
        //sitting
        [HttpGet]
        public IActionResult AddSitting()
        {
            ViewBag.SittingTypes = context.SittingTypes.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult AddSitting(Sitting sitting)
        {
            var a = _sittingRepository.GetSittingWithType().Where(e=>e.SittingTypeId == sitting.SittingTypeId).FirstOrDefault();
           
            //validate the time is not within in the existing list 


               var validateDate = _sittingRepository.ValidateDate(sitting);
                if (sitting.Start < sitting.End && sitting.Start< DateTime.Now.AddDays(90))
                {
                    if (validateDate)
                    {
                        _sittingRepository.AddSitting(sitting);
                        return RedirectToAction("SittingList");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "These Time already exists. Please choose another time.";
                        ViewBag.SittingTypes = context.SittingTypes.ToList();
                        return View(sitting);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Sorry,Start Time must be before End Time or schedule must be within 90 days. Please choose another time.";
                    ViewBag.SittingTypes = context.SittingTypes.ToList();
                    return View(sitting);
                }                
            
                         
        }

        public IActionResult Detail(int id)
        {

            Sitting model = _sittingRepository.GetSitting(id);
            return View(model);
        }
        public  IActionResult SittingList(string searchString)

        { 

            var sittings =  _sittingRepository.GetSittingWithType().ToList();

            if (!String.IsNullOrEmpty(searchString))
            {

                sittings = sittings.Where(s => s.SittingType.Description.Contains(searchString) || s.Start.ToShortDateString().Contains(searchString)||s.Capacity.ToString().Contains(searchString)).ToList();
            }
            return View(sittings);
        }
        [HttpPost]
        public IActionResult DeleteSelectedItems([FromBody]string[] SittingIds)
        {

            var sittings = _sittingRepository.GetSittingWithType().ToList();

                
            foreach(var id in SittingIds)
            {
                var s = sittings.Find(x=>x.SittingId == int.Parse(id));
                context.Sittings.Remove(s);
                
            }
            context.SaveChanges();
            return Json("All selected sittings deleted successfully!");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.SittingTypes = context.SittingTypes.ToList();
            var model = _sittingRepository.GetSitting(id);
            return View(model);         
        }
        [HttpPost]
        public RedirectToActionResult Edit(Sitting sitting)
        {
            Sitting updatedSitting = _sittingRepository.Update(sitting);
            return RedirectToAction("Detail", new { id = updatedSitting.SittingId });
        }
        [HttpPost]
        public RedirectToActionResult Delete(int id)
        {
            var model = _sittingRepository.GetSittingWithType().Where(e => e.SittingId == id).FirstOrDefault();
            var type = model.SittingType.Description;
            context.Sittings.Remove(model);
            context.SaveChanges();
            return RedirectToAction("SittingList", new { id = type });
        }

        //Sitting type
        public IActionResult SittingTypeList()
        {
            List<SittingType> sittingTypes = context.SittingTypes.ToList();
            return View(sittingTypes);
        }
        [HttpGet]
        public IActionResult AddSittingType()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSittingType(SittingType sittingType)
        {
           
            context.SittingTypes.Add(sittingType);
            context.SaveChanges();
            return RedirectToAction("DetailType", new { id = sittingType.Id });
        }
        [HttpGet]
        public IActionResult DeleteType(int id)
        {
            var model = context.SittingTypes.Find(id);
            return View(model);
        }
        [HttpPost]
        public RedirectToActionResult DeleteType(SittingType sittingType)
        {
            context.SittingTypes.Remove(sittingType);
            context.SaveChanges();
            return RedirectToAction("SittingTypeList");
        }
        [HttpGet]
        public IActionResult EditType(int id)
        {
            var model = context.SittingTypes.Find(id);
            return View(model);
        }
        [HttpPost]
        public RedirectToActionResult EditType(SittingType sittingType)
        {
            var _sittingType = context.SittingTypes.Attach(sittingType);
            _sittingType.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("DetailType", new { id = sittingType.Id });
        }
        public IActionResult DetailType(int id)
        {

            var model = context.SittingTypes.Find(id);
            return View(model);
        }
        [HttpGet]
        public JsonResult Test()
        {

            var data = context.Sittings.ToList();
          
            return Json(data);
            
        }

    }
}