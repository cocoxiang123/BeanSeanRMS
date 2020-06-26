using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T4RMSSolution.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SubId { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int Capacity { get; set; }
        

    }
}
