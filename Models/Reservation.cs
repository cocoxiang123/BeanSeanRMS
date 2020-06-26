
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using T4RMSSolution.Models;

namespace ReservationSystem.Models
{
    public class Reservation
    {       
        public int Id { get; set; }
        public int SittingId { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
        public int ReservationStatusId { get; set; }
        
        public ReservationStatus Status { get; set; }
        public Sitting Sitting { get; set; }
        public int ReservationTypeId { get; set; }
        public ReservationType ReservationType { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int Guests { get; set; }
        public DateTime DateTime { get; set; }     
        public string Notes { get; set; }
    }


}
