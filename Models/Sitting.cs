using ReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;

namespace ReservationSystem.Models
{
    public class Sitting
    {
        public Sitting()
        {
            Reservations = new List<Reservation>();

        }
        public int SittingId { get; set; }
        public int RestaurantId { get; set; }
        public bool Status { get; set; }
        //[DisplayFormat(DataFormatString = "{dd/mm/yyyy h:mm tt}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Time)]
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        public int Capacity { get; set; }
        public int SittingTypeId { get; set; }
        public SittingType SittingType { get; set; }

        public List<Reservation> Reservations { get; set; }

        //public DateTime Date { get; set; }
      

    }
}
