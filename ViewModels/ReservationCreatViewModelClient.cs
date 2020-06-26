using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ReservationSystem.ViewModels
{
    public class ReservationCreatViewModelClient
    {
        public int SittingId { get; set; }
        [Required(ErrorMessage = "Sitting need to be filled")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "The field is Required")]
        [StringLength(30) ]       
        public string FirstName { get; set; }
        [Required (ErrorMessage = "The field is Required")]
        [StringLength(30)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "The field is Required")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "The field is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "The field is Required")]
        public int Duration { get; set; }
        public int ReservationTypeId { get; set; }
        [Required(ErrorMessage = "The field is Required")]
        [Range(1,20)]
        public int Guests { get; set; }
        //[Required(ErrorMessage = "The field is Required")]
        //[DataType(DataType.DateTime)]

        //private DateTime _dateTime = DateTime.Now.Date;
        [DisplayFormat(DataFormatString = "dd/mm/yyyy h:mm tt")]
        [Required(ErrorMessage = "The field is Required")]
        public DateTime DateTime { get; set; }
        //public DateTime DateTime
        //{
        //    get
        //    {
        //        return  _dateTime;
        //    }
        //    set
        //    {
        //        _dateTime = value;
        //    }
        //}

        public string Notes { get; set; }
    }
}
