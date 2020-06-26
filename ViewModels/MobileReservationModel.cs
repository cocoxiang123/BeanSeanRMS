using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T4RMSSolution.ViewModels
{
    public class MobileReservationModel
    {
        public DateTime ReservationTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Guests { get; set; }
    }
}
