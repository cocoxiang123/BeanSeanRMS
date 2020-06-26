using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystem.Models
{
    public class Manager
    {
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
