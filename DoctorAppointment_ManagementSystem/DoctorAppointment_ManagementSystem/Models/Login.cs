using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorAppointment_ManagementSystem.Models
{
    public class Login
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is Required!")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is Required!")]
        public string Password { get; set; }
    }
}