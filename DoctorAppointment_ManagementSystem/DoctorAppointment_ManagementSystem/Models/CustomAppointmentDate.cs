using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace DoctorAppointment_ManagementSystem.Models
{
    public class CustomAppointmentDate : ValidationAttribute
    {
        public CustomAppointmentDate() :base("Previous dates can not be selected for appointment!")
        {

        }
        public override bool IsValid(object value)
        {

            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.Now; //Dates Greater than or equal to today are valid (true)
        }
    }
}