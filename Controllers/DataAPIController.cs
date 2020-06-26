using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReservationSystem.Models;
using T4RMSSolution.ViewModels;

namespace T4RMSSolution.Controllers
{
    public class DataAPIController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        public DataAPIController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        public List<ReportDataModel> ReservationsData()

        {
            var data = new List<ReportDataModel>();
            var reservations = _dbContext.Reservations.Select( a => new { Description = a.Sitting.SittingType.Description })
                .GroupBy(r=>r.Description)
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