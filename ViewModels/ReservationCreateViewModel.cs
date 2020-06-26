using ReservationSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T4RMSSolution.Models;


namespace ReservationSystem.ViewModels
{
    public class ReservationCreateViewModel
    {
        public ReservationCreateViewModel()
        {
            ReservationTypes = new List<ReservationType>();
            Tables = new List<Table>();
        }
        public int SittingId { get; set; }
        [Required(ErrorMessage = "Sitting need to be filled")]
        public int CustomerId { get; set; }
        [Display(Name = "Customer Name")]
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Phone]
        public string PhoneNumber { get; set; }

        public List<ReservationType> ReservationTypes { get; set; }
        public int TableId { get; set; }
        public List<Table> Tables { get; set; }
        public int ReservationTypeId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1, 12)]
        public int Guests { get; set; }
        [Required()]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$")]
        public string Email { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "when is the reservation")]

        private DateTime _dateTime = DateTime.MinValue;
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Required")]
        public DateTime DateTime
        {
            get
            {
                return (_dateTime==DateTime.MinValue?DateTime.Now.Date:_dateTime);
            }
            set
            {
                _dateTime = value;
            }
        }
    }
}
