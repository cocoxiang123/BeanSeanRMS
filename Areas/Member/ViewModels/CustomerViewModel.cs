using ReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T4RMSSolution.Models;

namespace T4RMSSolution.Areas.Member.ViewModels
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {    

        }
      [Display(Name = "ID")]
        public int ReservationId { get; set; }
        public DateTime Date { get; set; }
        public int Guest { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }


    }
}
