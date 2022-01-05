using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorAppointment_ManagementSystem.Models.ViewModels
{
    public class DocProfile
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
       
        public string Phone { get; set; }
        public System.DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string CNIC { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string specializationName { get; set; }

        public System.DateTime WorkingFrom { get; set; }
    }
}