using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationSystem.Models;
using ReservationSystem.Repos;
using T4RMSSolution.Areas.Member.ViewModels;

namespace ReservationSystem.Areas.Member.Controllers
{
    public class HomeController : MemberAreaController
    {
        private readonly ApplicationDbContext dbContext;
        UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;


        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager,ILogger<HomeController> logger)
           : base(context, userManager)
        {
            this.dbContext = context;
            this._userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {

            return View();
        }

       
        public async Task <IActionResult> MyOrder()
        {
            _logger.LogInformation(DateTime.Now.ToString()+"Log message in the MyOrder() method");
            var actionName = RouteData.Values["action"];
            _logger.LogInformation(DateTime.Now.ToString()+"action name:" + actionName.ToString());

            try
            {
            
                using (FileStream myFileStream = new FileStream("DebugFile.txt", FileMode.Append))
                {
                    TextWriterTraceListener listener = new TextWriterTraceListener(myFileStream);
                    Trace.Listeners.Add(listener);

                   Debug.WriteLine("Debugging in MyOrder");
                   var customer = await dbContext.Customers
                    .AsNoTracking()
                   .Include(c => c.Reservations)
                    .ThenInclude(r => r.Status)
                   .Include(c => c.Reservations)
                       .ThenInclude(r => r.ReservationType)
                   .Where(c => c.Email == User.Identity.Name)
                   .FirstOrDefaultAsync();
                   
                   
                    _logger.LogInformation(DateTime.Now.ToString()+"Customer Email :" + customer.Email);
                    Debug.Assert(customer is { }, "customer is null");
                    Debug.WriteLineIf(customer is null, "customer is null");
                    Debug.WriteLineIf(customer is { }, "Customer Identity=" + customer.Email);
                    Debug.Close();
                    Trace.Close();
                    return View(customer);
                }              
            }
            catch (Exception e)
            {
                _logger.LogError(DateTime.Now.ToString()+e.ToString());
                throw;             
            }
            
                             
        }
    }
}