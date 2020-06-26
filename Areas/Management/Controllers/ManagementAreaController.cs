using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = "Staff")]
    public class ManagementAreaController : Controller
    {
        
    }
}