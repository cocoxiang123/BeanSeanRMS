using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T4RMSSolution.Models
{
    public class ReservationStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set;}
    }
    
}
