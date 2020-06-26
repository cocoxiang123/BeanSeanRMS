using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ReservationSystem.Models;

namespace ReservationSystem.ViewModels
{
    public class ReservationEditViewModel
    {
       
        public Customer Customer { get; set; }
        
        public int SittingId { get; set; }

        public int CustomerId { get; set; }
       

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int ReservationStatusId { get; set; }
        public int ReservationTypeId { get; set; }

        [Required(ErrorMessage = "How many Guest")]
        public int Guests { get; set; }
        public string Email { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        [Required(ErrorMessage = "when is the reservation")]

        private DateTime _dateTime = DateTime.MinValue;
        [DataType(DataType.DateTime)]
        public DateTime DateTime
        {
            get
            {
                return (_dateTime == DateTime.MinValue ? DateTime.Now.Date : _dateTime);
            }
            set
            {
                _dateTime = value;
            }
        }
    }
}
