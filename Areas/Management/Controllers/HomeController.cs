using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem.Areas.Management.Controllers
{
    public class HomeController : ManagementAreaController
    {
        

        public IActionResult Index()
        {
            return View();
        }
    }
}