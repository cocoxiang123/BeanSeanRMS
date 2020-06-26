using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReservationSystem.Models;
using T4RMSSolution.Models;

namespace T4RMSSolution.ViewModels
{
    public class ReservationControlViewModel
    {
        public ReservationControlViewModel()
        {
            Reservations = new List<Reservation>();
           
        }

   
        public IEnumerable<Reservation> Reservations { get; set; }
    
    }
}
